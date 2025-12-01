using Injection;
using UnityEngine;

public class LaserDeviceBootstrap : MonoBehaviour
{

    [SerializeField] LaserDeviceScreen laserDeviceScreen;
    [SerializeField] NewLaserSignalButton newLaserSignalButton;
    [SerializeField] StartLaserTuningButton startLaserTuningButton;
    [SerializeField] StopLaserLineTuner stopLaserLineTuner;
    [SerializeField] LaserTunerLine horizontalLine;
    [SerializeField] LaserTunerLine verticalLine;
    [SerializeField] LaserTunerLine firstDiagonalLine;
    [SerializeField] LaserTunerLine secondDiagonalLine;
    [Inject] Injector injector;

    public void Initialize()
    {
        injector.Inject(horizontalLine);
        injector.Inject(verticalLine);
        injector.Inject(firstDiagonalLine);
        injector.Inject(secondDiagonalLine);
        injector.Inject(laserDeviceScreen);
        injector.Inject(newLaserSignalButton);
        injector.Inject(startLaserTuningButton);
        injector.Inject(stopLaserLineTuner);

        laserDeviceScreen.Init();
        newLaserSignalButton.InitStart();

    }
}
