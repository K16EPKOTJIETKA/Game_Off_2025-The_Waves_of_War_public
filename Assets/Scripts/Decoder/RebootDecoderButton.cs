using Injection;
using UnityEngine;

public class RebootDecoderButton : MonoBehaviour, IClickable
{
    [Inject] DecoderController controller;
    [SerializeField] ButtonAnimator animator;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        animator.Press();
        PlaySound();
        if (controller.isRebooting) return;
        controller.OnRebootingStarted();
    }
}
