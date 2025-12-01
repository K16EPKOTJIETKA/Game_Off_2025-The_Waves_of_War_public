using UnityEngine;

public class NextSignal : MonoBehaviour, IClickable
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
        Debug.Log(2);
        signalsScreen.NextSignal();
    }
}
