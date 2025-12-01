using UnityEngine;

public class StartCodingButton : MonoBehaviour, IClickable
{
    [SerializeField] CodeScreen codeScreen;
    [SerializeField] SignalsScreen signalsScreen;
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
        signalsScreen.OnStartedCoding();
        codeScreen.OutputText();
    }
}
