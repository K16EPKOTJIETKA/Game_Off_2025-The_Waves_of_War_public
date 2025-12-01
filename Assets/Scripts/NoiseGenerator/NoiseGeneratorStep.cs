using UnityEngine;
using Injection;

public class NoiseGeneratorStep : MonoBehaviour, IClickable
{
    [Inject] NoiseGenerator noiseGenerator;
    [SerializeField] int noiseLevel;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }

    public void OnClick()
    {
        PlaySound();
        noiseGenerator.SetLevel(noiseLevel);
    }
}
