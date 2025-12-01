using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] List<AudioClip> musicList = new List<AudioClip>();
    [SerializeField] MessageSystem messageSystem;

    IEnumerator PlayMusic()
    {
        while (true)
        {
            audioSource.clip = musicList[0];
            audioSource.Play();
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(60);
        }
        
    }

    IEnumerator PlayFinalMusic()
    {
        if (audioSource.isPlaying)
            yield return new WaitUntil(() => !audioSource.isPlaying);
        while (true)
        {
            audioSource.clip = musicList[1];
            audioSource.Play();
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(60);
        }
    }

    void StartPlayMusic()
    {
        StartCoroutine(PlayMusic());
    }

    void StartFinalButtle()
    {
        StopAllCoroutines();
        StartCoroutine(PlayFinalMusic());
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartPlayMusic();
    }

    private void OnEnable()
    {
        messageSystem.finalButtleStarted += StartFinalButtle;
    }
    private void OnDisable()
    {
        messageSystem.finalButtleStarted -= StartFinalButtle;
    }
}
