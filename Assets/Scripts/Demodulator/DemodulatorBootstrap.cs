using Injection;
using UnityEngine;

public class DemodulatorBootstrap : MonoBehaviour
{
    [SerializeField] GoToDecoderButton goToDecoderButton;
    [SerializeField] DemodulatorTextScreen demodulatorTextScreen;
    [SerializeField] DemodulatorSignalScreen demodulatorSignalScreen;
    [SerializeField] DemodulatorRebootButton demodulatorRebootButton;
    [SerializeField] AMButton amButton;
    [SerializeField] FMButton fmButtin;
    [SerializeField] FSKButton fskButton;
    [SerializeField] QAMButton qamButton;
    [Inject] Injector injector;
    public void Initialized()
    {
        
        injector.Inject(goToDecoderButton);
        injector.Inject(demodulatorTextScreen);
        injector.Inject(demodulatorSignalScreen);
        injector.Inject(demodulatorRebootButton);
        injector.Inject(amButton);
        injector.Inject(fmButtin);
        injector.Inject(fskButton);
        injector.Inject(qamButton);
 


        goToDecoderButton.Init();
        demodulatorTextScreen.Init();
        demodulatorSignalScreen.Init();
    }
}
