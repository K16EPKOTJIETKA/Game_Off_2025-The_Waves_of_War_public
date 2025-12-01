using Injection;
using UnityEngine;

public class StartLaserTuningButton : MonoBehaviour, IClickable
{
    [Inject] LaserDeviceController controller;
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
        if (!controller.canStartTuning || controller.isFinishing || controller.isFailing) return;
        controller.OnTuningStarted();
    }
}
