using UnityEngine;

[System.Serializable]
public class AMFormula : ISignalFormula
{
    public float ModulationIndex = 0.7f;

    public float GetValue(float t, SignalParameters parameters)
    {

        float Ac = parameters.Amplitude;         
        float fc = parameters.CarrierFrequency;  
        float fm = parameters.ModulationFrequency; 

        float m_t = Mathf.Sin(2f * Mathf.PI * fm * t);

        float envelope = Ac * (1.0f + ModulationIndex * m_t);

        float carrierWave = Mathf.Cos(2f * Mathf.PI * fc * t);

        float usefulSignal = envelope * carrierWave;

        float maxNoiseAmplitude = parameters.NoisePower;
        float randomNoise = Random.Range(-maxNoiseAmplitude, maxNoiseAmplitude);

        return usefulSignal + randomNoise;
    }
}
