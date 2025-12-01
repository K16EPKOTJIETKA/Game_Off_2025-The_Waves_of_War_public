using Injection;
using UnityEngine;

public class DemodulatorRebootButton : MonoBehaviour, IClickable
{
    [Inject] DemodulatorController controller;
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
        Debug.Log("Start rebooting...");
        controller.OnRebootingStarted();
    }
}
