using UnityEngine;

public class SendSignalButton : MonoBehaviour, IClickable
{
    [SerializeField] SignalsScreen signalsScreen;

    [SerializeField] Material baseMaterial;
    [SerializeField] Material canSendSignalMaterial;
    SignalSO currentSignal;
    CodeSO currentCode;
    MeshRenderer meshRenderer;
    ButtonAnimator buttonAnimator;

    bool canSend = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource completeSound;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        buttonAnimator.Press();
        PlaySound();
        if (!canSend) return;
        Debug.Log("Signal was sending");
        signalsScreen.OnSignalSent(currentCode, currentSignal);
        CantSendSignal();
    }

    void CanSendSignal(CodeSO code, SignalSO signal)
    {
        completeSound.Play();
        currentSignal = signal;
        currentCode = code;

        canSend = true;
        meshRenderer.material = canSendSignalMaterial;
    }

    void CantSendSignal()
    {
        meshRenderer.material = baseMaterial;
        canSend = false;
    }

    void ChangeCurrentSignal(SignalSO signal)
    {
        currentSignal = signal;
    }

    void CantSendSignal(SignalSO signal)
    {
        if (currentSignal != signal) return;
        meshRenderer.material = baseMaterial;
        canSend = false;
    }


    private void OnEnable()
    {
        CodeScreen.canSendSignalEvent += CanSendSignal;
        SignalsScreen.signalDeletedFromList += CantSendSignal;
        signalsScreen.startedCoding += CantSendSignal;
    }

    private void OnDisable()
    {
        CodeScreen.canSendSignalEvent -= CanSendSignal;
        SignalsScreen.signalDeletedFromList -= CantSendSignal;
        signalsScreen.startedCoding += CantSendSignal;
    }

    public void Init()
    {
        buttonAnimator = GetComponent<ButtonAnimator>();
        meshRenderer = GetComponent<MeshRenderer>();
        CantSendSignal();
    }

}
