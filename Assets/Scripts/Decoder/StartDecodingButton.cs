using Injection;
using UnityEngine;

public class StartDecodingButton : MonoBehaviour, IClickable
{
    [Inject] DecoderController decoderController;
    [SerializeField] ButtonAnimator buttonAnimator;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        buttonAnimator.Press();
        PlaySound();
        if (decoderController.currentSignalData == null) return;
        if (decoderController.isDecoding || decoderController.isError || decoderController.isRebooting) return;

        if (decoderController.currentSignalData.decoderCode == decoderController.currentCode)
            decoderController.OnDecodingStarted();
        else
            decoderController.OnWasError();
    }   
}
