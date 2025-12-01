using UnityEngine;
using Injection;

public class SetNewSignalButton : MonoBehaviour, IClickable
{
    SignalSO signal;
    bool canSetSignal = false;

    [Inject]
    SignalManager signalManager;
    [SerializeField] ButtonAnimator buttonAnimator;
    [SerializeField] AudioSource audioSource;

    void PlaySound()
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    public void OnClick()
    {
        buttonAnimator.Press();
        PlaySound();
        Debug.Log(3);
        if (canSetSignal)
        {
            Debug.Log(4);
            SetNewSignal(signal);
        }
    }

    void GetNewSignal(SignalSO newSignal)
    {
        signal = newSignal;
    }

    void SetNewSignal(SignalSO newSignal)
    {
        signalManager.OnNewSignalWasSet(newSignal);
    }

    void CanSetSignalChange(bool canSet)
    {
        canSetSignal = canSet;
    }

    private void OnEnable()
    {
        signalManager.newSignalEvent += GetNewSignal;
        NewSignalIndicator.canSetNewSignalChanged += CanSetSignalChange;
    }

    private void OnDisable()
    {
        signalManager.newSignalEvent -= GetNewSignal;
        NewSignalIndicator.canSetNewSignalChanged -= CanSetSignalChange;
    }

}
