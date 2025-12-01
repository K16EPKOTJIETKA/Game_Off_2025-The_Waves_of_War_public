using Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemodulatorController : MonoBehaviour
{
    public SignalSO currentSignalData { private set; get; }
    [SerializeField] int rebootingTime;
    [Inject] SignalManager signalManager;
    [Inject] ShopManager shopManager;
    [Inject] Injector injector;

    [SerializeField] public int frequencyAccuracy = 2;
    [SerializeField] public float amplitudeAccuracy = 0.1f;
    [SerializeField] TMP_Text rebootingPercent;

    [SerializeField] TMP_Text[] dirtyText;
    [SerializeField] TMP_Text[] incorrectText;

    Dictionary<int, SignalSO> completeSignals = new Dictionary<int, SignalSO>();

    public string outputText = "";

    public event Action activatedRightProtocol;
    public event Action wasError;
    public event Action rebootingStarted;
    public event Action rebootingEnded;
    public event Action canSendSignalEvent;
    public event Action<SignalSO, string> sentSignal;
    public event Action<SignalSO> newSignalWasSet;

    public bool isError { private set; get; } = false;
    public bool isRebooting { private set; get; } = false;
    public bool isDemodulating { private set; get; } = false;
    AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }

    public void OnSentSignal(SignalSO signalData, string demodulatorOutput)
    {
        sentSignal?.Invoke(signalData, demodulatorOutput);
        isDemodulating = false;
        currentSignalData = null;
    }

    public void OnCanSendSignalEvent()
    {
        canSendSignalEvent?.Invoke();
    }
    public void OnActivatedRightProtocol()
    {
        activatedRightProtocol?.Invoke();
        isDemodulating = true;
    }

    public void OnWasError()
    {
        wasError?.Invoke();
        isError = true;
        for (int i = 0; i < incorrectText.Length; i++)
        {
            incorrectText[i].gameObject.SetActive(true);
        }
    }

    public void OnWasDirtyError()
    {
        wasError?.Invoke();
        isError = true;
        for (int i = 0; i < dirtyText.Length; i++)
        {
           
            dirtyText[i].gameObject.SetActive(true);
        }
    }

    public void OnRebootingStarted()
    {
        for (int i = 0; i < dirtyText.Length; i++)
        {
            incorrectText[i].gameObject.SetActive(false);
            dirtyText[i].gameObject.SetActive(false);
        }
        rebootingStarted?.Invoke();
        Rebooting();
        isRebooting = true;

    }

    private void OnNewSignalWasSet(SignalSO signal)
    {
        newSignalWasSet?.Invoke(signal);
    }

    private void OnRebootingEnded()
    {
        rebootingEnded?.Invoke();
        isError = false;
        isRebooting = false;
        PlaySound();
    }
    void Rebooting()
    {
        if (isRebooting) return;
        StartCoroutine(RebootingRoutine());
    }

    void SetNewSignal(SignalParameters signalParameters, SignalSO signal)
    {
        if (signalParameters == null || signal == null) return;
        if (signalParameters.CarrierFrequency < signal.frequency - frequencyAccuracy ||
            signalParameters.CarrierFrequency > signal.frequency + frequencyAccuracy)
        {
            Debug.Log("Incorrect frequancy");
            OnWasDirtyError();
            return;
        }

        if (signalParameters.Amplitude < signal.amplitude - amplitudeAccuracy ||
            signalParameters.Amplitude > signal.amplitude + amplitudeAccuracy)
        {
            Debug.Log("Incorrect amplitude");
            OnWasDirtyError();
            return;
        }
       
        if (isDemodulating) OnRebootingStarted();

        currentSignalData = signal;
        if (completeSignals.ContainsKey(signal.id)) return;
        OnNewSignalWasSet(currentSignalData);
        completeSignals.Add(signal.id, signal);

    }

    void DecreaseRebootingTime(ShopItemData shopItem)
    {
        if (shopItem.itemID == 8)
        {
            rebootingTime -= (int)(rebootingTime * 0.2f);
        }
        if (shopItem.itemID == 9)
        {
            rebootingTime -= (int)(rebootingTime * 0.4f);
        }
    }

    IEnumerator RebootingRoutine()
    {
        rebootingPercent.gameObject.SetActive(true);
        for (int i = 0; i < 100; i++)
        {
            rebootingPercent.text = $"{i}%";
            yield return new WaitForSeconds((float)rebootingTime/100);
        }
        rebootingPercent.gameObject.SetActive(false);
        OnRebootingEnded();
    }

    private void OnEnable()
    {
        signalManager.signalFromAmplifaerWasGot += SetNewSignal;
        shopManager.productPurchased += DecreaseRebootingTime;
    }

    private void OnDisable()
    {
        signalManager.signalFromAmplifaerWasGot -= SetNewSignal;
        shopManager.productPurchased -= DecreaseRebootingTime;
    }

    public void Initialize()
    {
        injector.Inject(this);

        audioSource = GetComponent<AudioSource>();
    }

}
