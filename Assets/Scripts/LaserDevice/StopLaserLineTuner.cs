using Injection;
using System.Collections.Generic;
using UnityEngine;

public class StopLaserLineTuner : MonoBehaviour, IClickable
{
    [Inject] LaserDeviceController controller;
    [SerializeField] List<LaserTunerLine> lines = new List<LaserTunerLine>();
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
        if (!controller.isTuning) return;
        foreach (var line in lines)
        {
            if (!line.gameObject.activeSelf || line.isStopped)
                continue;
            line.Stop();
            return;
        }


    }
}
