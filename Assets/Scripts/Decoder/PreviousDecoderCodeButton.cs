using Injection;
using UnityEngine;

public class PreviousDecoderCodeButton : MonoBehaviour, IClickable
{
    [SerializeField] DecoderCodeChoiceScreen choiceScreen;
    [SerializeField] ButtonAnimator animator;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        PlaySound();
        animator.Press();
        Debug.Log(1);
        choiceScreen.PreviousCode();
    }

}
