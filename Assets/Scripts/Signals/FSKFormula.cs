using UnityEngine;

[System.Serializable]
public class FSKFormula : ISignalFormula
{
    [Tooltip("–азница частот, используема€ дл€ кодировани€ 1")]
    public float FrequencyShift = 10f; 

    [Tooltip("„астота модул€ции (битрейт) - сколько раз в секунду мен€етс€ бит")]
    public float BitRate = 2f;

    public float GetValue(float t, SignalParameters parameters)
    {
        float fc_mid = parameters.CarrierFrequency;

        float fc_low = fc_mid - FrequencyShift / 2f;
        float fc_high = fc_mid + FrequencyShift / 2f;

        float modulationCycle = 1f / BitRate;

        float timeInCycle = t % modulationCycle;

        float currentBit;
        if (timeInCycle < modulationCycle / 2f)
        {
            currentBit = 0f; 
        }
        else
        {
            currentBit = 1f; 
        }

        float instantFrequency;
        if (currentBit == 1f)
        {
            instantFrequency = fc_high;
        }
        else
        {
            instantFrequency = fc_low;
        }

        float usefulSignal = parameters.Amplitude * Mathf.Cos(2f * Mathf.PI * instantFrequency * t);

        float maxNoiseAmplitude = parameters.NoisePower;
        float randomNoise = Random.Range(-maxNoiseAmplitude, maxNoiseAmplitude);

        return usefulSignal + randomNoise;
    }
}
