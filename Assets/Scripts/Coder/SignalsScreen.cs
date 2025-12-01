using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Injection;
using System.Collections;

public class SignalsScreen : MonoBehaviour
{
    [Inject] DecoderController decoderController;
    [Inject] SignalsTimer signalsTimer;
    [Inject] PelengatorController pelengatorController;
    [Inject] BalanceSystem balanceSystem;
    [Inject] ShopManager shopManager;
    [Inject] CurrencyManager currencyManager;

    List<SignalNameUI> signalPanels = new List<SignalNameUI>();

    private Dictionary<int, TMP_Text> activeTimers = new Dictionary<int, TMP_Text>();
    public Dictionary<int, SignalSO> sendingSignals = new Dictionary<int, SignalSO>();

    [SerializeField] GameObject textPrefab;
    [SerializeField] Transform parentPanel;

    [SerializeField] Color baseTextColor;
    [SerializeField] Color currentTextColor;

    [SerializeField] int sendingTime = 60;

    private int currentSignalIndex = 0;
    private int previousSignalIndex;
    private int previousSignalChoice = -1;
    int chooseIndex;

    public static event Action<SignalSO> newSignalSelected;
    public static event Action<SignalSO> signalDeletedFromList;
    public static event Action<CodeSO, SignalSO> signalSent;
    public event Action startedCoding;

    public event Action<SignalSO> signalSentToAlly;
    public event Action<SignalSO, int> urgencySignalSentToAlly;

    void OnSignalSentToAlly(SignalSO signal)
    {
        signalSentToAlly?.Invoke(signal);
    }

    void OnUrgencySignalSentToAlly(SignalSO signal, int time)
    {
        urgencySignalSentToAlly?.Invoke(signal, time);
    }


    public void OnStartedCoding()
    {
        startedCoding?.Invoke();
    }

    private void OnNewSignalSelected(SignalSO newSignal)
    {
        newSignalSelected?.Invoke(newSignal);
    }

    private void OnSignalDeletedFromList(SignalSO signal)
    {
        signalDeletedFromList?.Invoke(signal);
    }

    public void OnSignalSent(CodeSO code, SignalSO signal)
    {
        signalSent?.Invoke(code, signal);
        StartSending(signal);

    }

    void StartSending(SignalSO signal)
    {
        if (signal == null)
        {
            Debug.LogWarning("StartSending: signal is null");
            return;
        }
        if (sendingSignals.ContainsKey(signal.id))
        {
            Debug.LogWarning($"Сигнал {signal.id} уже в процессе отправки!");
            return;
        }

        int panelIndex = signalPanels.FindIndex(p => p != null && p.signal != null && p.signal.id == signal.id);

        if (panelIndex == -1)
        {
            if (chooseIndex >= 0 && chooseIndex < signalPanels.Count && signalPanels[chooseIndex] != null && signalPanels[chooseIndex].signal == signal)
            {
                panelIndex = chooseIndex;
            }
            else
            {
                Debug.LogWarning($"StartSending: не найдена панель для сигнала {signal.id}");
                return;
            }
        }
        sendingSignals.Add(signal.id, signal);

        var panel = signalPanels[panelIndex];
        if (panel != null)
        {
            if (panel.image != null)
                panel.image.gameObject.SetActive(true);

            if (panel.signalName != null)
                panel.signalName.gameObject.SetActive(false);

            if (panel.sendingTimer != null)
                panel.sendingTimer.text = NumberToTime(sendingTime);
        }

        chooseIndex = panelIndex;
        StartCoroutine(SendingSignalRoutine(chooseIndex));
    }

    IEnumerator SendingSignalRoutine(int index)
    {
        pelengatorController.isSending = true;

        if (index < 0 || index >= signalPanels.Count)
        {
            pelengatorController.isSending = false;
            yield break;
        }

        var panel = signalPanels[index];
        var signal = panel.signal;

        for (int i = sendingTime; i >= 0; i--)
        {

            if (signalPanels.Contains(panel))
            {
                panel.sendingTimer.text = NumberToTime(i);
            }
            yield return new WaitForSeconds(pelengatorController.kN);

        } 
        if (!signal.isNoUrgency && signalsTimer.signalsTimers.TryGetValue(signal.id, out int value))
        {
            
            if (value < 0 && signalPanels.Count != 0)
            {
                balanceSystem.AddPointsToAlly(0);
                OnUrgencySignalSentToAlly(panel.signal, value);
           
            }
            else
            {
                if (signalPanels.Count != 0)
                {
                    balanceSystem.AddPointsToAlly(panel.signal.importance);
                    OnUrgencySignalSentToAlly(panel.signal, value);
                   
                }
                
            }
        }
        else
        {
            if (signalPanels.Count != 0)
            {
                balanceSystem.AddPointsToAlly(panel.signal.importance);
                OnSignalSentToAlly(panel.signal);
               
            }
            
        }

        if (signalPanels.Contains(panel))
            DeleteSignalFromList(signalPanels.IndexOf(panel));

        pelengatorController.isSending = false;
    }


    void DecreaseSendingTime(ShopItemData shopItem)
    {
        if (shopItem.itemID == 10)
        {
            sendingTime -= (int)(sendingTime * 0.2f);
        }
        if (shopItem.itemID == 11)
        {
            sendingTime -= (int)(sendingTime * 0.3f);
        }
        if (shopItem.itemID == 12)
        {
            sendingTime -= (int)(sendingTime * 0.5f);
        }
    }


    public void AddSignalToList(SignalSO newSignal)
    {

        if (signalPanels.Count >= 10)
        {

            if (signalPanels.Count > 0)
            {
                int oldSignalId = signalPanels[0].signal.id;
                if (activeTimers.ContainsKey(oldSignalId))
                {
                    activeTimers.Remove(oldSignalId);
                }

                signalPanels.RemoveAt(0);
            }
            if (signalPanels[0] != null)
            {
                Destroy(signalPanels[0].gameObject);
            }

            signalPanels.RemoveAt(0);
        }

        GameObject newSignalEntry = Instantiate(textPrefab, parentPanel);
        SignalNameUI uiItem = newSignalEntry.GetComponent<SignalNameUI>();



        if (uiItem != null)
        {
            signalPanels.Add(uiItem);
            uiItem.signal = newSignal;
            uiItem.signalName.text = newSignal.name;

            SignalSO currentSignal = signalPanels[signalPanels.Count - 1].signal;
            if (!currentSignal.isNoUrgency)
            {

                if (uiItem.timer != null)
                {
       
                    if (activeTimers.ContainsKey(currentSignal.id))
                        activeTimers.Remove(currentSignal.id);

                    activeTimers.Add(currentSignal.id, uiItem.timer);

                    uiItem.timer.text = " " + NumberToTime(signalsTimer.signalsTimers.GetValueOrDefault(currentSignal.id));

                }
            }

        }
    }

    string NumberToTime(int number)
    {
        int minutes = number / 60;
        int seconds = number - (minutes * 60);

        return $"{minutes}:{seconds}";


    }

    void UpdateSignalList()
    {
        foreach (var entry in activeTimers)
        {
            int signalId = entry.Key;       
            TMP_Text timerText = entry.Value; 

            if (timerText == null) continue;

            int timeRemaining = signalsTimer.signalsTimers.GetValueOrDefault(signalId);

            timerText.text = " " + NumberToTime(timeRemaining);

            if (timeRemaining <= 10)
                timerText.color = Color.red;

            if (timeRemaining <= 0)
            {
                timerText.text = " - ";
            }
        }
    }

    private void Update()
    {
        if (activeTimers.Count == 0) return;
        UpdateSignalList();
    }

    public void NextSignal()
    {
        if (signalPanels.Count == 0)
            return;


        int oldIndex = currentSignalIndex;


        currentSignalIndex++;
        if (currentSignalIndex >= signalPanels.Count)
            currentSignalIndex = 0;

        previousSignalIndex = oldIndex;


        if (signalPanels.Count == 1 && previousSignalIndex == currentSignalIndex)
        {

            var only = signalPanels[currentSignalIndex];
            if (only != null && only.signalName != null) only.signalName.color = currentTextColor;
            return;
        }

        if (previousSignalIndex >= 0 && previousSignalIndex < signalPanels.Count)
        {
            var prev = signalPanels[previousSignalIndex];
            if (prev != null)
            {
                if (prev.signalName != null) prev.signalName.color = baseTextColor;
                if (prev.timer != null) prev.timer.color = baseTextColor;
                if (prev.sendingTimer != null) prev.sendingTimer.color = baseTextColor;
                if (prev.image != null) prev.image.color = baseTextColor;
            }
        }
        if (currentSignalIndex >= 0 && currentSignalIndex < signalPanels.Count)
        {
            var cur = signalPanels[currentSignalIndex];
            if (cur != null)
            {
                if (cur.signalName != null) cur.signalName.color = currentTextColor;
                if (cur.timer != null) cur.timer.color = currentTextColor;
                if (cur.sendingTimer != null) cur.sendingTimer.color = currentTextColor;
                if (cur.image != null) cur.image.color = currentTextColor;
            }
        }
    }

    public void PreviousSignal()
    {
        if (signalPanels.Count == 0)
            return;

        int oldIndex = currentSignalIndex;

        currentSignalIndex--;
        if (currentSignalIndex < 0)
            currentSignalIndex = signalPanels.Count - 1;

        previousSignalIndex = oldIndex;

        if (signalPanels.Count == 1 && previousSignalIndex == currentSignalIndex)
        {
            var only = signalPanels[currentSignalIndex];
            if (only != null && only.signalName != null) only.signalName.color = currentTextColor;
            return;
        }

        if (previousSignalIndex >= 0 && previousSignalIndex < signalPanels.Count)
        {
            var prev = signalPanels[previousSignalIndex];
            if (prev != null)
            {
                if (prev.signalName != null) prev.signalName.color = baseTextColor;
                if (prev.timer != null) prev.timer.color = baseTextColor;
                if (prev.sendingTimer != null) prev.sendingTimer.color = baseTextColor;
                if (prev.image != null) prev.image.color = baseTextColor;
            }
        }

        if (currentSignalIndex >= 0 && currentSignalIndex < signalPanels.Count)
        {
            var cur = signalPanels[currentSignalIndex];
            if (cur != null)
            {
                if (cur.signalName != null) cur.signalName.color = currentTextColor;
                if (cur.timer != null) cur.timer.color = currentTextColor;
                if (cur.sendingTimer != null) cur.sendingTimer.color = currentTextColor;
                if (cur.image != null) cur.image.color = currentTextColor;
            }
        }
    }
    
    public void ChooseSignalButton()
    {
        if (signalPanels.Count == 0) return;

        if (currentSignalIndex < 0 || currentSignalIndex >= signalPanels.Count) return;

        if (previousSignalChoice >= 0 && previousSignalChoice < signalPanels.Count)
        {
            var prevPanel = signalPanels[previousSignalChoice];
            if (prevPanel != null && prevPanel.backGround != null)
                prevPanel.backGround.gameObject.SetActive(false);
        }

        var curPanel = signalPanels[currentSignalIndex];
        if (curPanel != null && curPanel.backGround != null)
            curPanel.backGround.gameObject.SetActive(true);

        chooseIndex = currentSignalIndex;
        previousSignalChoice = currentSignalIndex;

        if (curPanel != null && curPanel.signal != null)
            OnNewSignalSelected(curPanel.signal);
        else
            Debug.LogWarning("ChooseSignalButton: выбранная панель или её сигнал равны null");
    }

    public void DeleteSignalFromList()
    {
        if (signalPanels.Count == 0)
            return;

        int index = currentSignalIndex;

        SignalSO signalToDelete = signalPanels[index].signal;

        activeTimers.Remove(signalToDelete.id);

        var panel = signalPanels[index];
        if (panel != null)
            Destroy(panel.gameObject);

        signalPanels.RemoveAt(index);

        OnSignalDeletedFromList(signalToDelete);

        if (signalPanels.Count == 0)
            return;

        if (currentSignalIndex >= signalPanels.Count)
            currentSignalIndex = signalPanels.Count - 1;

        var current = signalPanels[currentSignalIndex];
        if (current != null && current.signalName != null)
            current.signalName.color = currentTextColor;
    }

    public void DeleteSignalFromList(int index)
    {
      

        if (signalPanels.Count == 0)
            return;

        if (index < 0 || index >= signalPanels.Count)
            return;

        SignalSO signalToDelete = signalPanels[index].signal;

        activeTimers.Remove(signalToDelete.id);

        var panel = signalPanels[index];
        if (panel != null)
            Destroy(panel.gameObject);

        signalPanels.RemoveAt(index);

        OnSignalDeletedFromList(signalToDelete);

        if (signalPanels.Count == 0)
            return;

        if (currentSignalIndex > index)
            currentSignalIndex--;

        if (currentSignalIndex >= signalPanels.Count)
            currentSignalIndex = signalPanels.Count - 1;

        var current = signalPanels[currentSignalIndex];
        if (current != null && current.signalName != null)
            current.signalName.color = currentTextColor;
    }

    void AddNewSignal(SignalSO newSignal)
    {
        AddSignalToList(newSignal);
        if (signalPanels.Count == 1)
        {
            signalPanels[0].signalName.color = currentTextColor;
            if (signalPanels[0].timer != null)
                signalPanels[0].timer.color = currentTextColor;
        }
    }

    private void OnEnable()
    {
        decoderController.sentSignal += AddNewSignal;
        shopManager.productPurchased += DecreaseSendingTime;
    }
    private void OnDisable()
    {
        decoderController.sentSignal -= AddNewSignal;
        shopManager.productPurchased -= DecreaseSendingTime;
    }
}
