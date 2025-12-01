using Injection;
using System.Collections;
using UnityEngine;

public class NewLaserSignalButton : MonoBehaviour
{
    [Inject] LaserDeviceController laserDeviceController;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material newLaserSignalMaterial;
    [SerializeField] int signalTime;
    [SerializeField] int blinkingTime;
    
   
    private bool canSetNewSignal;


    [Inject] SignalManager signalManager;
   

    [SerializeField] MeshRenderer meshRenderer;
    Material[] mats;
    private Coroutine blinkingCoroutine;

    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null || audioSource.isPlaying) return;
        audioSource.Play();
    }

    private void OnEnable()
    {
        signalManager.phaseChanged += StartLaserSignals;
        laserDeviceController.tuningStarted += OffIndicator;
    }
    private void OnDisable()
    {
        signalManager.phaseChanged -= StartLaserSignals;
        laserDeviceController.tuningStarted -= OffIndicator;
    }

    public void InitStart()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;
    }



    void StartLaserSignals(int i)
    {
        if (i == 1)
        {
            StartCoroutine(LaserSignalGettingRoutine());
        }
    }

    IEnumerator LaserSignalGettingRoutine()
    {
        while (true)
        {
            int time = UnityEngine.Random.Range(signalManager.minTimeBetweenLaserSignal, signalManager.maxTimeBetweenLaserSignal);
            yield return new WaitForSeconds(time);

            OnIndicator();

        }
    }

    void OnIndicator()
    {
        if (!laserDeviceController.deviceIsOn) return;
        if (blinkingCoroutine != null) StopCoroutine(blinkingCoroutine);

        blinkingCoroutine = StartCoroutine(IndicatorBlinking());
    }

    void OffIndicator()
    {
        if (blinkingCoroutine != null) StopCoroutine(blinkingCoroutine);

        if (mats.Length > 1)
        {
            mats[1] = baseMaterial;
            meshRenderer.materials = mats;
        }

        canSetNewSignal = false;
        laserDeviceController.ChangeCanStartTuning(canSetNewSignal);
    }

    IEnumerator IndicatorBlinking()
    {
        int time = 0;
        canSetNewSignal = true;
        laserDeviceController.ChangeCanStartTuning(canSetNewSignal);

        while (time < signalTime)
        {
            if (mats.Length > 1)
            {
                mats[1] = newLaserSignalMaterial; 
                meshRenderer.materials = mats;
            }
            PlaySound();
            yield return new WaitForSeconds(blinkingTime);
            time += blinkingTime;

            if (mats.Length > 1)
            {
                mats[1] = baseMaterial;
                meshRenderer.materials = mats;
            }

            yield return new WaitForSeconds(blinkingTime);
            time += blinkingTime;
        }

        OffIndicator();
    }
}
