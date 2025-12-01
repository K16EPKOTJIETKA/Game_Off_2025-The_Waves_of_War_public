using Injection;
using UnityEngine;

public class FSKButton : MonoBehaviour, IClickable
{
    [Inject] DemodulatorController controller;
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
        if (controller.isError || controller.isRebooting || controller.currentSignalData == null) return;
        if (controller.currentSignalData.demodulationProtocol == DemodulationProtocol.FSK_PSK)
        {
            Debug.Log("Ok");
            controller.OnActivatedRightProtocol();

        }
        else
        {
            Debug.Log("Error");
            controller.OnWasError();
        }
    }
}
