using Injection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalsTimer : MonoBehaviour
{
    [Inject] SignalManager signalManager;
    [Inject] Injector injector;

    public Dictionary<int, int> signalsTimers = new Dictionary<int, int>();

    public void Initialize()
    {
        injector.Inject(this);
    }

    void AddSignalWithTimer(SignalSO signal)
    {
        if (signal.isNoUrgency) return;
        signalsTimers.Add(signal.id, signal.urgency);
        StartCoroutine(Timer(signal.id));

    }

    IEnumerator Timer(int id)
    {
        while (signalsTimers.GetValueOrDefault(id) >= 0)
        {
            int timer = signalsTimers.GetValueOrDefault(id);
            yield return new WaitForSeconds(1);
            timer--;
            signalsTimers[id] = timer;
        }
    
    }

    private void OnEnable()
    {
        signalManager.newSignalWasSet += AddSignalWithTimer;
    }

    private void OnDisable()
    {
        signalManager.newSignalWasSet -= AddSignalWithTimer;
    }
}
