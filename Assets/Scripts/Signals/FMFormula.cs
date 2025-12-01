using UnityEngine;

[System.Serializable]
public class FMFormula : ISignalFormula
{
    public float FrequencyDeviation = 5.0f;

    public float GetValue(float t, SignalParameters parameters)
    {
        float modulatingSignal = Mathf.Sin(2f * Mathf.PI * parameters.ModulationFrequency * t);

        float instantFrequency = parameters.CarrierFrequency + FrequencyDeviation * modulatingSignal;

        float maxNoiseAmplitude = parameters.NoisePower;
        float randomNoise = Random.Range(-maxNoiseAmplitude, maxNoiseAmplitude);
        return parameters.Amplitude * Mathf.Cos(2f * Mathf.PI * instantFrequency * t) + randomNoise;
    }

}
