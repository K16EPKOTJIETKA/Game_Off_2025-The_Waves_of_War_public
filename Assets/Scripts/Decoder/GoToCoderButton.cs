using Injection;
using UnityEngine;


public class GoToCoderButton : MonoBehaviour, IClickable
{
    [Inject] DecoderController decoderController;

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
    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = baseMat;
        animator = GetComponent<ButtonAnimator>();
    }
    public void OnClick()
    {
        animator.Press();
        PlaySound();
        if (!canSend || decoderController.currentSignalData == null) return;
        meshRenderer.material = baseMat;
        canSend = false;
        decoderController.OnSentSignal(decoderController.currentSignalData);
    }

    void CanSend()
    {
        meshRenderer.material = canSendMat;
        completeSound.Play();
        canSend = true;
    }

    void Reboot()
    {

        meshRenderer.material = baseMat;
        canSend = false;

    }

    private void OnEnable()
    {
        decoderController.canSendSignalEvent += CanSend;
        decoderController.rebootingStarted += Reboot;
    }

    private void OnDisable()
    {
        decoderController.canSendSignalEvent -= CanSend;
        decoderController.rebootingStarted -= Reboot;
    }
}
