using Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalManager : MonoBehaviour
{
    [Inject] Injector injector;
    [Inject] SatelliteTunerController satelliteTunerController;
    [Inject] LaserDeviceController laserDeviceController;
    [Inject] ShopManager shopManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] List<SignalSO> radiowaveSignalsList = new List<SignalSO>();
    [SerializeField] List<SignalSO> microwaveSignalsList = new List<SignalSO>();
    [SerializeField] List<SignalSO> laserSignalsList = new List<SignalSO>();
    public Dictionary<int, SignalSO> signals = new Dictionary<int, SignalSO>();
    [SerializeField] int minNewSignalTime;
    [SerializeField] int maxNewSignalTime;
    [SerializeField] int startChance = 40;
    int k = 0;
    [SerializeField] int microwaveSignalGettingTime = 60;

    [SerializeField] int laserSignalNullTime = 10;
    [SerializeField] int microwaveSignalNullTime = 10;
    [SerializeField] public int minTimeBetweenLaserSignal = 240;
    [SerializeField] public int maxTimeBetweenLaserSignal = 375;
    [SerializeField] int minSatelliteTime = 45;
    [SerializeField] int maxSatelliteTime = 75;

    SignalSO currentSignal;

    List<SignalSO>[] radioPhase = new List<SignalSO>[5];
    List<SignalSO>[] microPhase = new List<SignalSO>[5];
    List<SignalSO>[] laserPhase = new List<SignalSO>[5];
    public int[] maxPhaseScore = new int[5];

    int currentPhase = 0;

    private int currentSignalId = -1;

    public event Action<SignalSO> newSignalEvent;
    public event Action<SignalSO> newSignalWasSet;
    public event Action<SignalParameters, SignalSO> signalFromAmplifaerWasGot;
    public event Action<int> phaseChanged;
    public event Action<SignalSO> newLaserSignalEvent;

    void InitSignalsPhases()
    {
        for (int i = 0; i < 5; i++)
        {
            radioPhase[i] = new List<SignalSO>();
            microPhase[i] = new List<SignalSO>();
            laserPhase[i] = new List<SignalSO>();
        }
        int radioStep = 0;
        int microStep = 0;
        int laserStep = 0;
        for (int i = 0; i < 5;i++)
        { 
            for (int j = 0 + radioStep; j < 12 + radioStep; j++)
            {
                if (j >= radiowaveSignalsList.Count) break;
                radioPhase[i].Add(radiowaveSignalsList[j]);
                maxPhaseScore[i] += radiowaveSignalsList[j].importance > 0 ? radiowaveSignalsList[j].importance : 0;
            }
            radioStep += 12;
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0 + microStep; j < 6 + microStep; j++)
            {
                if (j >= microwaveSignalsList.Count) break;
                microPhase[i].Add(microwaveSignalsList[j]);
                maxPhaseScore[i] += microwaveSignalsList[j].importance > 0 ? microwaveSignalsList[j].importance : 0;
            }
            microStep += 6;
        }

        for (int i = 1; i < 5; i++)
        {
            for (int j = 0 + laserStep; j < 4 + laserStep; j++)
            {
                if (j >= laserSignalsList.Count) break;
                laserPhase[i].Add(laserSignalsList[j]);
                maxPhaseScore[i] += laserSignalsList[j].importance > 0 ? laserSignalsList[j].importance : 0;
            }
            laserStep += 4;
        }
    }
    private void OnNewSignalEvent(SignalSO signal)
    {
        newSignalEvent?.Invoke(signal);
        DeleteSignal(signal);
        if (signal.signalType != SignalType.RadioWave) return;
        ChooseRadiowaveSignal();
    }

    private void OnNewLaserSignalEvent(SignalSO laserSignal)
    {
        newLaserSignalEvent?.Invoke(laserSignal);
    }

    public void OnNewSignalWasSet(SignalSO signal)
    {
        newSignalWasSet?.Invoke(signal);
        
    }

    public void OnSignalFromAmplifaerWasGot(SignalParameters currentSignal, SignalSO signal)
    {
        signalFromAmplifaerWasGot?.Invoke(currentSignal, signal);
    }

    IEnumerator GetNewSignal(int newSignal)
    {
        int time = UnityEngine.Random.Range(minNewSignalTime, maxNewSignalTime);
        if (firstSignal)
        {
            yield return new WaitForSeconds(time / 2);
            firstSignal = false;
        }
        else yield return new WaitForSeconds(time);

        if (currentPhase >= radioPhase.Length) yield break;

        if (radioPhase[currentPhase].Count == 0)
        {
            
            PhaseChanger();
            yield break; 
        }

        if (currentSignal != null && currentSignal.signalType == SignalType.Laser)
        {
            yield return new WaitForSeconds(laserSignalNullTime);
        }
        if (currentSignal != null && currentSignal.signalType == SignalType.MicroWave)
        {
            yield return new WaitForSeconds(microwaveSignalNullTime);
        }

        if (newSignal >= radioPhase[currentPhase].Count)
        {
            newSignal = UnityEngine.Random.Range(0, radioPhase[currentPhase].Count);
        }
        
        if (radiowaveSignalsList.Count != 0 && radioPhase[currentPhase].Count != 0)
        {
            var signalFromPhase = radioPhase[currentPhase][newSignal];

            currentSignal = signalFromPhase; 

            OnNewSignalEvent(signals.GetValueOrDefault(signalFromPhase.id));
            radioPhase[currentPhase].Remove(signalFromPhase);

            
        }
        PhaseChanger();
    }

    IEnumerator NullSignalTimer(int nullTime)
    {
        yield return new WaitForSeconds(nullTime);
        currentSignal = null;
    }

    void ChooseLaserSignal()
    {
        if (laserSignalsList.Count == 0 || currentPhase == 0 || currentPhase == 5) return;

        int signalNumber = UnityEngine.Random.Range(0, laserPhase[currentPhase].Count);
 
        OnNewSignalEvent(signals.GetValueOrDefault(laserPhase[currentPhase][signalNumber].id));
        
    }

    IEnumerator LaserSignalGettingRoutine(int signalNumber)
    {
        int time = UnityEngine.Random.Range(minTimeBetweenLaserSignal, maxTimeBetweenLaserSignal);
        yield return new WaitForSeconds(time);

        if (signalNumber >= laserPhase[currentPhase].Count)
        {
            signalNumber = UnityEngine.Random.Range(0, laserPhase[currentPhase].Count);
        }

        if (laserPhase[currentPhase].Count != 0)
        {
            currentSignal = laserPhase[currentPhase][signalNumber];
            OnNewLaserSignalEvent(signals.GetValueOrDefault(laserPhase[currentPhase][signalNumber].id));
            laserPhase[currentPhase].Remove(laserPhase[currentPhase][signalNumber]);
        }
        
        
    }

    void ChooseMicrowaveSignal()
    {
        if (microwaveSignalsList.Count == 0 || currentPhase == 5) return;
        int chance = UnityEngine.Random.Range(0, 100);
        if (chance > startChance + k) return;

        int signalNumber = UnityEngine.Random.Range(0, microPhase[currentPhase].Count);

        StartCoroutine(MicrowaveSignalGettingRoutine(signalNumber));
        
    }

    IEnumerator MicrowaveSignalGettingRoutine(int signalNumber)
    {
        int time = UnityEngine.Random.Range(minSatelliteTime, maxSatelliteTime);
        yield return new WaitForSeconds(microwaveSignalGettingTime);
        if (currentSignal != null && currentSignal.signalType == SignalType.Laser)
        {
            yield return new WaitForSeconds(laserSignalNullTime);
        }

        if (signalNumber >= microPhase[currentPhase].Count)
        {
            signalNumber = UnityEngine.Random.Range(0, microPhase[currentPhase].Count);
        }

        if (microPhase[currentPhase].Count != 0)
        {
            currentSignal = microPhase[currentPhase][signalNumber];
            OnNewSignalEvent(signals.GetValueOrDefault(microPhase[currentPhase][signalNumber].id));
            microPhase[currentPhase].Remove(microPhase[currentPhase][signalNumber]);
        }
        

    }
    bool firstSignal = true;
    void ChooseRadiowaveSignal()
    {
        if (radioPhase[currentPhase].Count == 0 || currentPhase == 5) return;

        int signalNumber = UnityEngine.Random.Range(0, radioPhase[currentPhase].Count);
      
        StartCoroutine(GetNewSignal(signalNumber));
        
    }


    void DeleteSignal(SignalSO signal)
    {
        signals.Remove(signal.id);
        switch (signal.signalType)
        {
            case SignalType.RadioWave:
                radiowaveSignalsList.Remove(signal);
                break;
            case SignalType.MicroWave:
                microwaveSignalsList.Remove(signal);
                break;
            case SignalType.Laser:
                laserSignalsList.Remove(signal);
                break;
            default:
                break;
        }
    }


    void InitializeSignalsDictionary()
    {
        for (int i = 0; i < radiowaveSignalsList.Count; i++)
        {
            signals.Add(radiowaveSignalsList[i].id, radiowaveSignalsList[i]);
        }
        for (int i = 0; i < microwaveSignalsList.Count; i++)
        {
            signals.Add(microwaveSignalsList[i].id, microwaveSignalsList[i]);
        }
        for (int i = 0; i < laserSignalsList.Count; i++)
        {
            signals.Add(laserSignalsList[i].id, laserSignalsList[i]);
        }
    }

    public void Initialize()
    {
        injector.Inject(this);
        InitializeSignalsDictionary();
        InitSignalsPhases();
        ChooseRadiowaveSignal();
        
    }

    void PhaseChanger()
    {
        if (radioPhase[currentPhase].Count == 0)
        {
            StartCoroutine(PhaseChangingRoutine());
        }
    }

    void OnPhaseChanged(int phaseNumber)
    {
        phaseChanged.Invoke(phaseNumber);
        if (phaseNumber == 1)
        {
            StartCoroutine(LaserSignalGettingRoutine(0));
        }
    }

    IEnumerator PhaseChangingRoutine()
    {
        yield return new WaitForSeconds(90);
        currentPhase++;
        OnPhaseChanged(currentPhase);
    }
    
    void UpgradeK(ShopItemData shopItem)
    {
        if (shopItem.itemID == 1)
        {
            k += 30;
        }
    }
    private void OnEnable()
    {
        satelliteTunerController.tuningFinished += ChooseMicrowaveSignal;
        laserDeviceController.tuningFinished += ChooseLaserSignal;
        shopManager.productPurchased += UpgradeK;

    }

    private void OnDisable()
    {
        satelliteTunerController.tuningFinished -= ChooseMicrowaveSignal;
        laserDeviceController.tuningFinished -= ChooseLaserSignal;
        shopManager.productPurchased -= UpgradeK;
    }
}
