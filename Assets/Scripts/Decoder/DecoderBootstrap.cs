using Injection;
using UnityEngine;

public class DecoderBootstrap : MonoBehaviour
{
    [SerializeField] GoToCoderButton goToCoderButton;
    [SerializeField] DecoderCodeChoiceScreen decoderCodeChoiceScreen;
    [SerializeField] DemodulatorOutputScreen demodulatorOutputScreen;
    [SerializeField] DecoderInfoScreen decoderInfoScreen;
    [SerializeField] StartDecodingButton startDecodingButton;
    [SerializeField] RebootDecoderButton rebootDecoderButton;
    [Inject] Injector injector;
    public void Initialize()
    {
        injector.Inject(goToCoderButton);
        injector.Inject(decoderInfoScreen);
        injector.Inject(decoderCodeChoiceScreen);
        injector.Inject(demodulatorOutputScreen);
        injector.Inject(startDecodingButton);
        injector.Inject(rebootDecoderButton);

        goToCoderButton.Init();
        decoderCodeChoiceScreen.Init();
        demodulatorOutputScreen.Init();
        decoderInfoScreen.Init();
    }
}
