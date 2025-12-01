using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MessageScreen : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;
    [SerializeField] Button readButton;
    [SerializeField] Renderer indicator;

    [SerializeField] float typeSpeed = 0.02f;
    [SerializeField] float blinkSpeed = 0.3f;

    [SerializeField] Material offIndicatorMat;
    [SerializeField] Material onIndicatorMat;

    public bool isTyping;

    Coroutine typeRoutine;
    Coroutine blinkRoutine;

    [SerializeField] MessageSystem system;

    Material[] mats;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource readSound;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }


    public void Init()
    {

        if (messageText == null)
            messageText = GetComponentInChildren<TMP_Text>(true);

        if (readButton == null)
            readButton = GetComponentInChildren<Button>(true);

        readButton.onClick.RemoveAllListeners();
        readButton.onClick.AddListener(OnRead);

        readButton.gameObject.SetActive(false);

        mats = indicator.materials;
    }

    public void ShowMessage(string msg)
    {
        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        readButton.gameObject.SetActive(true);
        readButton.interactable = false;

        typeRoutine = StartCoroutine(Type(msg));
    }

    IEnumerator Type(string msg)
    {
        messageText.text = "";
        isTyping = true;

        for (int i = 0; i < msg.Length; i++)
        {
            messageText.text = msg.Substring(0, i + 1);
            PlaySound();
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        readButton.interactable = true;
    }

    public void Clear()
    {
        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        messageText.text = "";
        readButton.gameObject.SetActive(false);
        readButton.interactable = false;
        isTyping = false;
    }

    public void OnRead()
    {
        readSound.Play();
        system.OnMessageRead();
    }

    public void StartBlink()
    {
        if (blinkRoutine != null) return;
        blinkRoutine = StartCoroutine(Blink());
    }

    public void StopBlink()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }

        mats[1] = offIndicatorMat;
        indicator.materials = mats;
    }

    IEnumerator Blink()
    {
        bool st = false;

        while (true)
        {
            st = !st;
            mats[1] = st ? onIndicatorMat : offIndicatorMat;
            indicator.materials = mats;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}


