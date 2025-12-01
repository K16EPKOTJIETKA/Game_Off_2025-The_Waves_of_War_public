using System.Collections.Generic;
using UnityEngine;
using Injection;

public class StopLineButton : MonoBehaviour, IClickable
{
    [Inject] SatelliteTunerController controller;
    [SerializeField] List<TunerLine> lines = new List<TunerLine>();
    [SerializeField] ButtonAnimator buttonAnimator;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        buttonAnimator.Press();
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
