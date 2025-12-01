using UnityEngine;
using Injection;

public class SatelliteTunerBootstrap : MonoBehaviour
{
    [SerializeField] SatelliteTunerScreen satelliteTunerScreen;
    [SerializeField] StopLineButton stopLineButton;
    [SerializeField] TunerLine horizontalLine;
    [SerializeField] TunerLine verticalLine;
    [SerializeField] StartSatelliteTuning startSatelliteTuning;
    [SerializeField] SatelliteIndicators satelliteIndicators;
    [SerializeField] SatelliteTunerController satelliteTunerController;
    [Inject] Injector injector;
    public void Initialize()
    {
        injector.Inject(stopLineButton);
        injector.Inject(horizontalLine);
        injector.Inject(verticalLine);
        injector.Inject(satelliteTunerScreen);
        injector.Inject(startSatelliteTuning);
        injector.Inject(satelliteTunerController);
        injector.Inject(satelliteIndicators);


        satelliteTunerScreen.Init();
    }
}
