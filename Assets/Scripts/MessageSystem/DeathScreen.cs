using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] PelengatorController pelengatorController;
    [SerializeField] Image image;
    [SerializeField] float duration = 10;
    AudioSource audioSource;

    Tween tween;

    void StartDeath()
    {
        audioSource.Play();
        tween = image.DOFade(0.4f, duration);

    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        pelengatorController.playerDeathStarted += StartDeath;
    }
    private void OnDisable()
    {
        pelengatorController.playerDeathStarted += StartDeath;
    }
}
