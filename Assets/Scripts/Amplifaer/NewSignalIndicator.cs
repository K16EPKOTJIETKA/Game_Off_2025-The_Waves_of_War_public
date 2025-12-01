using System;
using System.Collections;
using UnityEngine;
using Injection;

public class NewSignalIndicator : MonoBehaviour
{
    [SerializeField] Material baseMaterial;
    [SerializeField] Material newRadiowaveSignalMaterial;
    [SerializeField] Material newMicrowaveSignalMaterial;
    [SerializeField] Material newLaserSignalMaterial;
    [SerializeField] int signalTime;
    [SerializeField] int blinkingTime;
    private bool canSetNewSignal;

    private Material newSignalMaterial;

    public static event Action<bool> canSetNewSignalChanged;

    [Inject] SignalManager signalManager;

    AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null || audioSource.isPlaying) return;
        audioSource.Play();
    }

    private void OnCanSetNewSignalChanged(bool canSet)
    {
        canSetNewSignalChanged?.Invoke(canSet);
    }


    MeshRenderer meshRenderer;
    
    private void OnEnable()
    {
        signalManager.newSignalEvent += OnIndicator;
        signalManager.newSignalWasSet += OffIndicator;
    }
    private void OnDisable()
    {
        signalManager.newSignalEvent -= OnIndicator;
        signalManager.newSignalWasSet += OffIndicator;
    }

    void OnIndicator(SignalSO signalSO)
    {
        StopAllCoroutines();
        newSignalMaterial = ChooseMaterial(signalSO);
        StartCoroutine(IndicatorBlinking());
    }

    Material ChooseMaterial(SignalSO signal)
    {
        switch (signal.signalType)
        {
            case SignalType.RadioWave:
                audioSource.pitch = 0.7f;
                return newRadiowaveSignalMaterial;
            case SignalType.MicroWave:
                audioSource.pitch = 1f;
                return newMicrowaveSignalMaterial;
            case SignalType.Laser:
                audioSource.pitch = 1.5f;
                return newLaserSignalMaterial;
            default:
                return baseMaterial;
        }
    }
    Material[] mats;

    void OffIndicator(SignalSO signalSO)
    {
        StopAllCoroutines();
        mats[1] = baseMaterial;
        meshRenderer.materials = mats;
        canSetNewSignal = false;
        OnCanSetNewSignalChanged(canSetNewSignal);
    }


    IEnumerator IndicatorBlinking()
    {
        int time = 0;
        canSetNewSignal = true;
       
        OnCanSetNewSignalChanged(canSetNewSignal);
        while (time != signalTime)
        {
            mats[1] = newSignalMaterial;
            meshRenderer.materials = mats;
            PlaySound();
            Debug.Log(1);
            yield return new WaitForSeconds(blinkingTime);
            time += blinkingTime;
            mats[1] = baseMaterial;
            meshRenderer.materials = mats;
            yield return new WaitForSeconds(blinkingTime);
            time += blinkingTime;
        }
        mats[1] = baseMaterial;
        meshRenderer.materials = mats;
        canSetNewSignal = false;
        OnCanSetNewSignalChanged(canSetNewSignal);
    }

    public void InitStart()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;
        audioSource = GetComponent<AudioSource>();
    }
}
