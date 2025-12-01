using UnityEngine;

[CreateAssetMenu(fileName = "SignalSO", menuName = "Scriptable Objects/SignalSO")]
public class SignalSO : ScriptableObject
{
    public int id;
    public int importance;
    public Formula formula;

    public int necessaryReinforcement;


    public float amplitude;

    public int frequency;
    public SignalType signalType;
    public DemodulationProtocol demodulationProtocol;
    public CodeSO decoderCode;

    public int countOfDemodulationCharacters;

    public bool isPicture;
    public string text;
    public Sprite image;

    public bool isNoUrgency;

    public int urgency;
    public float noisePower;

    public SignalParameters GetParameters(float currentTime)
    {

        return new SignalParameters
        {
            Id = id,
            CarrierFrequency = this.frequency,
            Amplitude = this.amplitude,
            ModulationFrequency = this.frequency * 0.01f, 

            Time = currentTime,
            NoisePower = noisePower
        };
    }
}

public enum Formula
{
    AmplitudeModulation, // AM
    FrequencyModulation, // FM
    FrequencyShiftKeying, // FSK
    QuadratureAmplitudeModulation, // QAM
}
public enum SignalType
{
    RadioWave,
    MicroWave,
    Laser,
}

public enum DemodulationProtocol
{
    AM,
    FM,
    FSK_PSK,
    QAM,
}

public enum DecoderCode
{
    NoCode,
    CaesarCipher,
    VigenèreCipher,
    Enigma,
    AES,
}


