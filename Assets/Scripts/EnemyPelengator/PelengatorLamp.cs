using UnityEngine;
using Injection;
using System.Collections;

public class PelengatorLamp : MonoBehaviour
{
    [Inject] PelengatorController controller;

    [SerializeField] Material baseMat;
    [SerializeField] Material indicateMat;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] int blinkingTime;
    Material[] mats;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null || audioSource.isPlaying) return;
        audioSource.Play();
    }


    void IndicatorOn()
    {
        mats = meshRenderer.materials;
        IndicatorOff();
        StartCoroutine(IndicateRoutine());
    }

    void IndicatorOnFull()
    {
        mats[1] = indicateMat;
        meshRenderer.materials = mats;
        audioSource.Stop();
    }

    void IndicatorOff()
    {
        StopAllCoroutines();
        mats[1] = baseMat;
        meshRenderer.materials = mats;
    }

    IEnumerator IndicateRoutine()
    {
        while (controller.isPelengating)
        {
            mats[1] = indicateMat;
            meshRenderer.materials = mats;
            PlaySound();
            yield return new WaitForSeconds(blinkingTime);
            mats[1] = baseMat;
            meshRenderer.materials = mats;
            yield return new WaitForSeconds(blinkingTime);
        }
        mats[1] = baseMat;
        meshRenderer.materials = mats;
    }

    private void OnEnable()
    {
        controller.pelengatingStarted += IndicatorOn;
        controller.pelengatingFinished += IndicatorOff;
        controller.playerDeathStarted += IndicatorOnFull;
    }
    private void OnDisable()
    {
        controller.pelengatingStarted -= IndicatorOn;
        controller.pelengatingFinished -= IndicatorOff;
        controller.playerDeathStarted -= IndicatorOnFull;
    }
}
