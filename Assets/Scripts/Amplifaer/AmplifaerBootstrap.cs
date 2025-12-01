using UnityEngine;
using Injection;

public class AmplifaerBootstrap : MonoBehaviour
{
    [SerializeField] CursorPoint cursorPoint;
    [SerializeField] NewSignalIndicator newSignalIndicator;
    [SerializeField] LineGraphRendere lineGraphRendere;
    [SerializeField] SetNewSignalButton setNewSignalButton;
    [SerializeField] AmplifaerSignalSettings amplifaerSignalSettings;
    [SerializeField] SetSignalToDemodulatorBitton signalToDemodulatorBitton;
    [Inject] Injector injector;

    public void Initialize()
    {
        injector.Inject(lineGraphRendere);
        injector.Inject(setNewSignalButton);
        injector.Inject(amplifaerSignalSettings);
        
        injector.Inject(newSignalIndicator);
        injector.Inject(signalToDemodulatorBitton);
        Debug.Log("We injected");

        lineGraphRendere.InitAwake();
        cursorPoint.InitStart();
        newSignalIndicator.InitStart();
        
    }


}
