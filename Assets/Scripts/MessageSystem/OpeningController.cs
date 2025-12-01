using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OpeningController : MonoBehaviour
{
    AudioSource audioSource;
    Tween tween;
    [SerializeField] Image image;
    [SerializeField] float duration = 1;
    [SerializeField] PlayerController playerController;
    void PlaySound()
    {
        if (audioSource == null || audioSource.isPlaying) return;
        audioSource.Play();
    }

    void OpenScene()
    {
        tween = image.DOFade(0, duration).OnComplete(() => playerController.enabled = true);

    }

    private void Start()
    {
        playerController.enabled = false;
        audioSource = GetComponent<AudioSource>();
        OpenScene();
    }
}
