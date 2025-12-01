using UnityEngine;

public class PreviousCodeButton : MonoBehaviour, IClickable
{
    [SerializeField] CodeChoiceScreen choiceScreen;
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
        Debug.Log(1);
        choiceScreen.PreviousCode();
    }

}
