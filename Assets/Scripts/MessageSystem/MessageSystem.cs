using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Injection;
using System;

[RequireComponent(typeof(AudioSource))]
public class MessageSystem : MonoBehaviour
{
    [SerializeField] List<MessageSO> messageSOs = new List<MessageSO>();
    [SerializeField] MessageScreen screen;
    [SerializeField] int sendingTime = 30;
    [SerializeField] int welcomeMessageTime = 15;
    [SerializeField] int finalButtleStartTime = 15;
    [SerializeField] SignalsScreen signalsScreen;
    [Inject] BalanceSystem balanceSystem;
    [Inject] CurrencyManager currencyManager;
    AudioSource audioSource;

    Dictionary<int, MessageSO> messageDictionary = new Dictionary<int, MessageSO>();

    public float soundTime = 1f;

    Queue<string> messages = new Queue<string>();
    bool showing;

    public event Action gameWasEnd;
    public event Action finalButtleStarted;

    void OnGameWasEnd()
    {
        gameWasEnd?.Invoke();
    }

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (var message in messageSOs)
        {
            messageDictionary.Add(message.id, message);
        }
        StartCoroutine(WelcomeMessage());
    }

    IEnumerator WelcomeMessage()
    {
        yield return new WaitForSeconds(welcomeMessageTime);
        ReceiveMessage(messageDictionary[0].message);
    }

    public void ReceiveMessage(string msg)
    {
        messages.Enqueue(msg);

        screen.StartBlink();
        StartCoroutine(PlaySound());

        if (!showing)
            ShowNext();
    }

    public IEnumerator ReceiveBadMessageRoutine()
    {
        yield return new WaitForSeconds(sendingTime);
        ReceiveMessage(messageDictionary[3].message);
    }

    public IEnumerator ReceiveNeutralMessageRoutine()
    {
        yield return new WaitForSeconds(sendingTime);
        ReceiveMessage(messageDictionary[2].message);
    }

    public IEnumerator ReceiveGoodMessageRoutine(SignalSO signal)
    {
        yield return new WaitForSeconds(sendingTime);
        currencyManager.AddCurrency(signal.importance);
        ReceiveMessage(messageDictionary[1].message);
    }

    void ReceiveSignal(SignalSO signalSO)
    {
        if (signalSO.importance > 0)
        {
            StartCoroutine(ReceiveGoodMessageRoutine(signalSO));
            
        }
        else
        {
            StartCoroutine(ReceiveBadMessageRoutine());
        }
    }

    void ReceiveUrgencySignal(SignalSO signalSO, int time)
    {
        if (signalSO.importance > 0 && time > 0)
        {
            StartCoroutine(ReceiveGoodMessageRoutine(signalSO));
        }
        else if (signalSO.importance > 0 && time <= 0)
        {
            StartCoroutine(ReceiveNeutralMessageRoutine());
        }
        else StartCoroutine(ReceiveBadMessageRoutine());
    }

    void ShowNext()
    {
        if (messages.Count == 0)
        {
            showing = false;
            screen.Clear();
            screen.StopBlink();
            return;
        }

        showing = true;

        string msg = messages.Dequeue();
        screen.ShowMessage(msg);
    }

    public void OnMessageRead()
    {
        if (screen.isTyping)
            return;

        if (isEnd)
            StartCoroutine(EndGameRoutine());
        ShowNext();
    }

    IEnumerator PlaySound()
    {
      
        for (int i = 0; i < 3; i++)
        {
            audioSource.Play();
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        audioSource.Stop();
    }

    bool isEnd;
    void GetPhaseEndingMessage(int phase, bool result)
    {
        switch (phase)
        {
            case 0:
                if (result)
                    ReceiveMessage(messageDictionary[4].message);
                else ReceiveMessage(messageDictionary[5].message);
                break;
            case 1:
                if (result)
                    ReceiveMessage(messageDictionary[6].message);
                else ReceiveMessage(messageDictionary[7].message);
                break;
            case 2:
                if (result)
                    ReceiveMessage(messageDictionary[8].message);
                else ReceiveMessage(messageDictionary[9].message);
                break;
            case 3:
                if (result)
                    ReceiveMessage(messageDictionary[10].message);
                else ReceiveMessage(messageDictionary[11].message);
                StartCoroutine(FinalButtleStart());
                break;
            case 4:
                if (result)
                    ReceiveMessage(messageDictionary[13].message);
                else ReceiveMessage(messageDictionary[14].message);
                isEnd = true;
                break;
            default:
                break;
        }
    }

    IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(15);
        OnGameWasEnd();
    }

    IEnumerator FinalButtleStart()
    {
        yield return new WaitForSeconds(finalButtleStartTime);
        ReceiveMessage(messageDictionary[12].message);
        finalButtleStarted?.Invoke();
    }

    private void OnEnable()
    {
        signalsScreen.signalSentToAlly += ReceiveSignal;
        signalsScreen.urgencySignalSentToAlly += ReceiveUrgencySignal;
        balanceSystem.phaseEndingWasGot += GetPhaseEndingMessage;
    }

    private void OnDisable()
    {
        signalsScreen.signalSentToAlly -= ReceiveSignal;
        signalsScreen.urgencySignalSentToAlly -= ReceiveUrgencySignal;
        balanceSystem.phaseEndingWasGot -= GetPhaseEndingMessage;
    }
}

