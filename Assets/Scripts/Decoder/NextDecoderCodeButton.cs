using UnityEngine;

public class NextDecoderCodeButton : MonoBehaviour, IClickable
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
        animator.Press();
        PlaySound();
        Debug.Log(0);
        choiceScreen.NextCode();
    }
}
