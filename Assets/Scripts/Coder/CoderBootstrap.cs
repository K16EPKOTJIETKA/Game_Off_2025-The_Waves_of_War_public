using Injection;
using UnityEngine;

public class CoderBootstrap : MonoBehaviour
{
    [SerializeField] CodeChoiceScreen codeChoiceScreen;
    [SerializeField] SendSignalButton sendSignalButton;
    [SerializeField] SignalsScreen signalsScreen;
    [Inject] Injector injector;
    public void Initialize()
    {
        injector.Inject(signalsScreen);
        injector.Inject(codeChoiceScreen);
        codeChoiceScreen.Init();
        sendSignalButton.Init();
    }
}
