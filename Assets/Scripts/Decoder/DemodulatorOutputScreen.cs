using Injection;
using TMPro;
using UnityEngine;

public class DemodulatorOutputScreen : MonoBehaviour
{
    [Inject] DecoderController decoderController;
    [SerializeField] TMP_Text text;

    [SerializeField] Material baseMat;
    [SerializeField] Material rebootMat;

    MeshRenderer meshRenderer;


    void ChangeOutputText()
    {
        text.text = decoderController.demodulatorOutput;
    }

    void CleanScreen(SignalSO signal)
    {
        text.text = string.Empty;
    }

    public void Init()
    {
        ChangeOutputText();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        decoderController.rebootingStarted += Reboot;
        decoderController.rebootingFinished += RebootEnd;
        decoderController.newSignalWasSet += ChangeOutputText;
        decoderController.sentSignal += CleanScreen;
    }

    private void OnDisable()
    {
        decoderController.rebootingStarted -= Reboot;
        decoderController.rebootingFinished -= RebootEnd;
        decoderController.newSignalWasSet -= ChangeOutputText;
        decoderController.sentSignal -= CleanScreen;
    }

    void Reboot()
    {
        //meshRenderer.material = rebootMat;
        text.gameObject.SetActive(false);
    }

    void RebootEnd()
    {
        //meshRenderer.material = baseMat;
        text.gameObject.SetActive(true);
    }
}
