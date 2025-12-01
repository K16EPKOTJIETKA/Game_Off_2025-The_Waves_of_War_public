using DG.Tweening;
using Injection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    [SerializeField] List<MessageSO> endings = new List<MessageSO>();
    Dictionary<int, MessageSO> endingsDictionary = new Dictionary<int, MessageSO>();

    [SerializeField] TMP_Text text;
    [SerializeField] Image blackPanel;
    [SerializeField] int duration = 5;
    [SerializeField] float symbolTypingTime = 0.05f;
    [SerializeField] LoadingScreen loadingScreen;
    [SerializeField] GameObject loadingCanvas;
    [Inject] BalanceSystem balanceSystem;
    [SerializeField] MessageSystem messageSystem;
    [Inject] PelengatorController pelengatorController;
    [Inject] Injector injector;
    
    int currentEnding = 0;

    AudioSource audioSource;
    public AudioMixer audioMixer;
    public string volumeParameter = "MasterVolume";

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }

    public void Initialize()
    {
        injector.Inject(this);

        foreach (var ending in endings)
        {
            endingsDictionary.Add(ending.id, ending);
        }
        audioSource = GetComponent<AudioSource>();
    }
    private Tween _fadeTween;
    void ShowEnding()
    {
        audioMixer.SetFloat(volumeParameter, -80f);
        _fadeTween = blackPanel.DOFade(1f, duration)
            .SetLink(gameObject) 
            .OnComplete(() =>
            {
                
                if (currentEnding == 0)
                    StartCoroutine(TypeText(endingsDictionary[15].message));
                else if (currentEnding == 1)
                    StartCoroutine(TypeText(endingsDictionary[16].message));
                else if (currentEnding == 2)
                    StartCoroutine(TypeText(endingsDictionary[17].message));
            });
    }

    IEnumerator TypeText(string str)
    {
        text.text = string.Empty;
        for (int i = 0; i < str.Length; i++)
        {
            text.text += str[i];
            PlaySound();
            yield return new WaitForSeconds(symbolTypingTime);
        }
        yield return new WaitForSeconds(duration);
        StartCoroutine(ShowQuote(endingsDictionary[18].message));

    }

    IEnumerator ShowQuote(string str)
    {
        text.text = string.Empty;
        for (int i = 0; i < str.Length; i++)
        {
            text.text += str[i];
            PlaySound();
            yield return new WaitForSeconds(symbolTypingTime);
        }
        yield return new WaitForSeconds(duration);
        StartCoroutine(ShowThanks(endingsDictionary[19].message));
    }

    IEnumerator ShowThanks(string str)
    {
        text.text = string.Empty;
        for (int i = 0; i < str.Length; i++)
        {
            text.text += str[i];
            PlaySound();
            yield return new WaitForSeconds(symbolTypingTime);
        }
        yield return new WaitForSeconds(duration);
        text.text = "";
        loadingCanvas.SetActive(true);
        loadingScreen.StartLoading("MainMenu");
    }

    void ChangeEnding(int ending)
    {
        currentEnding = ending;
    }

    void DeathEnding()
    {
        _fadeTween = blackPanel.DOFade(1f, 0f)
           .SetLink(gameObject)
           .OnComplete(() =>
           {
               audioMixer.SetFloat(volumeParameter, -80f);
               StartCoroutine(TypeText(endingsDictionary[20].message));
           });
    }

    private void OnEnable()
    {
        balanceSystem.gameEndingWasGot += ChangeEnding;
        messageSystem.gameWasEnd += ShowEnding;
        pelengatorController.playerDeadEvent += DeathEnding;
    }
    private void OnDisable()
    {
        balanceSystem.gameEndingWasGot -= ChangeEnding;
        messageSystem.gameWasEnd -= ShowEnding;
        pelengatorController.playerDeadEvent -= DeathEnding;
    }

}
