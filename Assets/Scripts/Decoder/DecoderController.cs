using System;
using System.Collections;
using UnityEngine;
using Injection;
using TMPro;

public class DecoderController : MonoBehaviour
{
    public SignalSO currentSignalData { private set; get; }
    [SerializeField] public int rebootingTime;
    [Inject] private DemodulatorController demodulatorController;
    [Inject] ShopManager shopManager;
    [Inject] Injector injector;

    [SerializeField] TMP_Text rebootingPercent;
    [SerializeField] TMP_Text incorrectText;
    public CodeSO currentCode { private set; get; }
    public string demodulatorOutput { private set; get; } = "";
    public bool isError { private set; get; } = false;
    public bool isRebooting { private set; get; } = false;
    public bool isDecoding { private set; get; } = false;

    public event Action<CodeSO> codeChanged;
    public event Action decodingStarted;
    public event Action decodingFinished;
    public event Action rebootingStarted;
    public event Action rebootingFinished;
    public event Action wasError;
    public event Action canSendSignalEvent;
    public event Action<SignalSO> sentSignal;
    public event Action newSignalWasSet;

    AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    private void GetNewSignal(SignalSO newSignal, string text)
    {
        if (isDecoding) OnRebootingStarted();

        currentSignalData = newSignal;
        demodulatorOutput = text;
        OnNewSignalWasSet();
    }

    void OnNewSignalWasSet()
    {
        newSignalWasSet?.Invoke();
    }

    public void OnSentSignal(SignalSO signal)
    {
        sentSignal?.Invoke(signal);
        isDecoding = false;
        currentSignalData = null;   
    }
    public void OnCanSendSignalEvent()
    {
        canSendSignalEvent?.Invoke();
        
    }

    public void OnCodeChanged(CodeSO code)
    {
        codeChanged?.Invoke(code);
        currentCode = code;
    }

    public void OnDecodingStarted()
    {
        decodingStarted?.Invoke();
        isDecoding = true;
    }

    public void OnDecodingFinished()
    {
        decodingStarted?.Invoke();
        isDecoding = false;
    }

    public void OnRebootingStarted()
    {
        incorrectText.gameObject.SetActive(false);
        rebootingStarted?.Invoke();
        isRebooting = true;
        isDecoding = false;
        StartCoroutine(RebootindRoutine());
    }

    private void OnRebootingFinidhed()
    {
        rebootingFinished?.Invoke();
        isRebooting = false;
        isError = false;
        PlaySound();
    }

    public void OnWasError()
    {
        wasError?.Invoke();
        incorrectText.gameObject.SetActive(true);
        isError = true;
    }


    void ChangeCurrentCode(CodeSO code)
    {
        currentCode = code;
    }

    private void OnEnable()
    {
        demodulatorController.sentSignal += GetNewSignal;
        shopManager.productPurchased += DecreaseRebootingTime;
    }

    private void OnDisable()
    {
        demodulatorController.sentSignal -= GetNewSignal;
        shopManager.productPurchased -= DecreaseRebootingTime;
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
    IEnumerator RebootindRoutine()
    {
        rebootingPercent.gameObject.SetActive(true);
        for (int i = 0; i < 100; i++)
        {
            rebootingPercent.text = $"{i}%";
            yield return new WaitForSeconds((float)rebootingTime / 100);
        }
        rebootingPercent.gameObject.SetActive(false);
        OnRebootingFinidhed();
    }

    public void Initialize()
    {
        injector.Inject(this);

        audioSource = GetComponent<AudioSource>();
    }
}
