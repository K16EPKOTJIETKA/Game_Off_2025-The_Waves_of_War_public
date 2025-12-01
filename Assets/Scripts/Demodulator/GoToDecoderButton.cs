using Injection;
using UnityEngine;

public class GoToDecoderButton : MonoBehaviour, IClickable
{
    [Inject] DemodulatorController controller;

    [SerializeField] Material baseMat;
    [SerializeField] Material canSendMat;

    MeshRenderer meshRenderer;
    ButtonAnimator animator;

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
        animator.Press();
        PlaySound();
        if (!canSend || controller.currentSignalData == null) return;
        controller.OnSentSignal(controller.currentSignalData, controller.outputText);
        canSend = false;
        meshRenderer.material = baseMat;
    }

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<ButtonAnimator>();
    }

    void CanSendSignal()
    {
        canSend = true;
        meshRenderer.material = canSendMat;
        completeSound.Play();
    }

    void Reboot()
    {
        meshRenderer.material = baseMat;
        canSend = false;

    }

    private void OnEnable()
    {
        controller.canSendSignalEvent += CanSendSignal;
        controller.rebootingStarted += Reboot;
    }

    private void OnDisable()
    {
        controller.canSendSignalEvent -= CanSendSignal;
        controller.rebootingStarted -= Reboot;
    }


}
