using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    [SerializeField] TMP_Text textInfo;
    [SerializeField] Image imageInfo;
    SignalSO currentSignal;

    private void OnEnable()
    {
        SignalsScreen.newSignalSelected += UpdateInfo;
        SignalsScreen.signalDeletedFromList += DeleteInfo;
    }

    private void OnDisable()
    {
        SignalsScreen.newSignalSelected -= UpdateInfo;
        SignalsScreen.signalDeletedFromList -= DeleteInfo;
    }

    void UpdateInfo(SignalSO signal)
    {
        if (currentSignal == signal) return;

        currentSignal = signal;

        if (signal.isPicture)
        {
            textInfo.text = "";
            imageInfo.sprite = signal.image;
            imageInfo.gameObject.SetActive(true);
            return;
        }
        imageInfo.gameObject.SetActive(false);
        textInfo.text = signal.text;
    }

    void DeleteInfo(SignalSO deleteSignal)
    {
        if (currentSignal != deleteSignal) return;
        Debug.Log("Delete info...");
        textInfo.text = "";
        imageInfo.gameObject.SetActive(false);
    }
}
