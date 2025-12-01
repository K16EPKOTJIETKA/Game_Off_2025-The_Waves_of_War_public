using UnityEngine;
using Injection;

public class AmplifaerSignalSettings : MonoBehaviour
{
    private SignalSO signalSO;
    [SerializeField] private LineGraphRendere LineGraphRendere;
    private SignalParameters currentSignal;

    [Inject] SignalManager signalManager;

    [Range(0,100)]
    public int signalReinforcement;

    [Range(10,250)]
    public int signalFrequency;

    [SerializeField]
    private float tuningFrequencySensitivity = 50f;

    [SerializeField]
    private float tuningReinforcementSensitivity = 10f;


    private float maxNoise;


    private void OnEnable()
    {
        Debug.Log("AmplifaerSignalSettings enable");
        signalManager.newSignalWasSet += SetNewSignal;
    }

    private void OnDisable()
    {
        signalManager.newSignalWasSet -= SetNewSignal;
    }

    void SetNewSignal(SignalSO newSignal)
    {
        signalSO = newSignal;
        currentSignal = LineGraphRendere.currentSignalParameters;
        maxNoise = signalSO.noisePower;
    }

    void Update()
    {
        TuneFrequency();
        TuneReinforcement();

    }
    
    void TuneFrequency()
    {
        if (signalSO == null || currentSignal == null) return;

        float difference = Mathf.Abs(signalSO.frequency - signalFrequency);

        float normalizedError = difference / tuningFrequencySensitivity;

        float clampedError = Mathf.Clamp01(normalizedError);

        float noiseFactor = Mathf.Sin(clampedError * Mathf.PI / 2f);

        //currentSignal.NoisePower = noiseFactor;
        LineGraphRendere.currentSignalParameters.NoisePower = noiseFactor;
    }

    void TuneReinforcement()
    {
        if (signalSO == null || currentSignal == null) return;

        float difference = Mathf.Abs(signalSO.necessaryReinforcement - signalReinforcement);

        float normalizedError = difference / tuningReinforcementSensitivity;

        float clampedError = Mathf.Clamp01(normalizedError);

        float amplitude = Mathf.Cos(clampedError * Mathf.PI / 2f);

        //currentSignal.Amplitude = amplitude * signalSO.amplitude;

        LineGraphRendere.currentSignalParameters.Amplitude = amplitude * signalSO.amplitude;


    }
}
