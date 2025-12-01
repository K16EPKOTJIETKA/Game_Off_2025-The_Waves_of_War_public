using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Injection;

public class SatelliteIndicators : MonoBehaviour
{
    [Inject] SignalManager signalManager;
    [SerializeField] SatelliteTunerController satelliteTunerController;
    [SerializeField] List<MeshRenderer> indicators = new List<MeshRenderer>();
    [SerializeField] int signalTime;
    [SerializeField] int blinkingTime;
    int currentIndex = 0;
    int newIndex = -1;
    bool canStartTuning;
    [SerializeField] Material baseMat;
    [SerializeField] Material indicatorMat;

    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null || audioSource.isPlaying) return;
        audioSource.Play();
    }

    private Coroutine activeCoroutine;
    void ChooseIndicator(SignalSO signal)
    {
        if (signal.signalType != SignalType.RadioWave) return;
        StopCurrentBlinking();
        newIndex = Random.Range(0, indicators.Count); 
        currentIndex = newIndex;
        OnIndicator();
      
    }

    void StopCurrentBlinking()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        if (currentIndex != -1 && currentIndex < indicators.Count)
        {
            SetMaterial(currentIndex, baseMat);
        }

        satelliteTunerController.ChangeCanStartTuning(false);
    }


    void SetMaterial(int index, Material mat)
    {
        Material[] mats = indicators[index].materials;

        if (mats.Length > 1)
        {
            mats[1] = mat;
            indicators[index].materials = mats;
        }
    }

    Material[] mats;
    Coroutine coroutine;
    void OnIndicator()
    {
        mats = indicators[currentIndex].materials;
        activeCoroutine = StartCoroutine(IndicatorBlinking(currentIndex));
    }

    void OffIndicator() 
    {
        StopCoroutine(coroutine);
        mats[1] = baseMat;
        indicators[currentIndex].materials = mats;

        canStartTuning = false;
        satelliteTunerController.ChangeCanStartTuning(canStartTuning);
    }
    IEnumerator IndicatorBlinking(int index)
    {
        satelliteTunerController.ChangeCanStartTuning(true);

        float timer = 0;

        while (timer < signalTime)
        {
            PlaySound();
            SetMaterial(index, indicatorMat);
            yield return new WaitForSeconds(blinkingTime);

            SetMaterial(index, baseMat);
            yield return new WaitForSeconds(blinkingTime);

            timer += blinkingTime * 2;
        }

        StopCurrentBlinking();

    }

    IEnumerator IndicatorBlinking()
    {
        int time = 0;
        canStartTuning = true;
        satelliteTunerController.ChangeCanStartTuning(canStartTuning);
        while (time != signalTime)
        {
            mats[1] = indicatorMat;
            indicators[currentIndex].materials = mats;
            Debug.Log(1);
            yield return new WaitForSeconds(blinkingTime);
            time += blinkingTime;
            mats[1] = baseMat;
            indicators[currentIndex].materials = mats;
            yield return new WaitForSeconds(blinkingTime);
            time += blinkingTime;
        }
        mats[1] = baseMat;
        indicators[currentIndex].materials = mats;
        canStartTuning = false;
        satelliteTunerController.ChangeCanStartTuning(canStartTuning);
    }

    private void OnEnable()
    {
        signalManager.newSignalEvent += ChooseIndicator;
        satelliteTunerController.tuningStarted += StopCurrentBlinking;
    }

    private void OnDisable()
    {
        signalManager.newSignalEvent -= ChooseIndicator;
        satelliteTunerController.tuningStarted -= StopCurrentBlinking; 
    }

}
