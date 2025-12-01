using UnityEngine;

[System.Serializable]
public class QAMFormula : ISignalFormula
{
    [Tooltip("Масштаб дискретных амплитудных шагов")]
    public float SymbolScale = 0.5f;


    public float GetValue(float t, SignalParameters parameters)
    {
        float fc = parameters.CarrierFrequency;
        float Ac = parameters.Amplitude;
        float Rs = parameters.ModulationFrequency; 

        if (Rs <= 0) return 0f; 

        float Ts = 1f / Rs;

        int symbolIndex = Mathf.FloorToInt(t / Ts);

        float I_t;
        float Q_t; 

        if (symbolIndex % 2 == 0)
            I_t = SymbolScale;
        else
            I_t = -SymbolScale;

        if (symbolIndex % 4 < 2)
            Q_t = SymbolScale;
        else
            Q_t = -SymbolScale;

        float cosWave = Mathf.Cos(2f * Mathf.PI * fc * t);
        float sinWave = Mathf.Sin(2f * Mathf.PI * fc * t);

        float usefulSignal = Ac * (I_t * cosWave - Q_t * sinWave);

        float maxNoiseAmplitude = parameters.NoisePower;
        float randomNoise = Random.Range(-maxNoiseAmplitude, maxNoiseAmplitude);

        return usefulSignal + randomNoise;
    }
}
