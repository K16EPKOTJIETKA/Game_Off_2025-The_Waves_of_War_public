
[System.Serializable]
public class SignalParameters 
{
    public int Id;
    public int CarrierFrequency; 
    public float Amplitude;
    public float ModulationFrequency; 

    public float Time;
    public float NoisePower;

    public void SetTime(float time)
    {
        this.Time = time;
    }
}
