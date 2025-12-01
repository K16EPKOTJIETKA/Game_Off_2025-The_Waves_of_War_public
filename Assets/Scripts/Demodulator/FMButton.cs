using Injection;
using UnityEngine;


public class FMButton : MonoBehaviour, IClickable
{
    [Inject] DemodulatorController controller;
    [SerializeField] ButtonAnimator animator;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        animator.Press();
        PlaySound();
        if (controller.isError || controller.isRebooting || controller.currentSignalData == null) return;
        if (controller.currentSignalData.demodulationProtocol == DemodulationProtocol.FM)
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
