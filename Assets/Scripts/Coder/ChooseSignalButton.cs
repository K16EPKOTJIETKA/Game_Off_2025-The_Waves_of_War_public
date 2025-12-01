using UnityEngine;

public class ChooseSignalButton : MonoBehaviour, IClickable
{
    [SerializeField] SignalsScreen signalsScreen;
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
        Debug.Log(5);
        signalsScreen.ChooseSignalButton();
    }
}
