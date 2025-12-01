using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Injection;

public class PelengatorController : MonoBehaviour
{
    [Inject] NoiseGenerator noiseGenerator;
    [Inject] EnemyController enemyController;
    [Inject] Injector injector;


    CodeSO currentCode;
    [SerializeField] int sendingSignalTime = 60;
    [SerializeField] float pelengatingSpeed;
    [SerializeField] float decreasingPelengatorSpeed;
    [SerializeField] Image pelengator;
    [SerializeField] int timeToDeath = 15;

    [SerializeField] PelengatorLamp pelengatorLamp;

    [SerializeField] Color level1;
    [SerializeField] Color level2;
    [SerializeField] Color level3;
    [SerializeField] Color level4;



    public bool isSending = false;
    public bool enemyNearStation = false;
    public bool isPelengating = false;

    [SerializeField] int pelengatingTime = 10;
    int pelengatingChance;

    int kS = 1;
    public int kN { private set; get; } = 1;

    Coroutine pelengatingByEnemy;
    Coroutine pelengatingBySignal;

    private Coroutine activePelengationRoutine;

    public event Action<float> pelengatorLevelWasChanged;
    public event Action pelengatingStarted;
    public event Action pelengatingFinished;
    public event Action<int> sendingTimeChenged;
    public event Action playerDeadEvent;
    public event Action playerDeathStarted;

    void OnSendingTimeChenged(int i)
    {
        sendingTimeChenged?.Invoke(i);
    }

    bool isEnd;
    void OnPelengatorLevelWasChanged(float lvl)
    {
        pelengatorLevelWasChanged?.Invoke(lvl);
        if (lvl >= 1 && !isEnd)
        {
            isEnd = true;
            playerDeathStarted?.Invoke();
            return;
        }
        if (lvl > 0.25 && lvl <= 0.5) pelengator.color = level2;
        else if (lvl > 0.5 && lvl <= 0.75) pelengator.color = level3;
        else if (lvl > 0.75 && lvl <= 1) pelengator.color = level4;
        else pelengator.color = level1;
    }

    IEnumerator StartDeathRoutine()
    {
        yield return new WaitForSeconds(timeToDeath);
        playerDeadEvent?.Invoke();
    }

    public void OnPlayerDeadEvent()
    { 
        playerDeadEvent?.Invoke(); 
    }

    void OnPelengatingStarted()
    {
        isPelengating = true;
        pelengatingStarted?.Invoke();
    }

    void OnPelengatingFinished()
    {
        isPelengating = false;
        pelengatingFinished?.Invoke();
    }

    void SetNewCode(CodeSO code, SignalSO signal)
    {
        currentCode = code;
    }

    public void Initialize()
    {
        injector.Inject(this);
        injector.Inject(pelengatorLamp);
    }

    void ChoosePelengatingChance()
    {
        switch (currentCode.codeStrength)
        {
            case CodeStrength.NoStrength:
                pelengatingChance = 80;
                break;
            case CodeStrength.Bad:
                pelengatingChance = 40;
                break;
            case CodeStrength.Medium:
                pelengatingChance = 20;
                break;
            case CodeStrength.Good:
                pelengatingChance = 10;
                break;
            case CodeStrength.VeryGood:
                pelengatingChance = 1;
                break;
            default:
                pelengatingChance = 80;
                break;
        }
    }

    void ChoosePelengatingChance(CodeSO code)
    {
        switch (code.codeStrength)
        {
            case CodeStrength.NoStrength:
                pelengatingChance = 80;
                break;
            case CodeStrength.Bad:
                pelengatingChance = 40;
                break;
            case CodeStrength.Medium:
                pelengatingChance = 20;
                break;
            case CodeStrength.Good:
                pelengatingChance = 10;
                break;
            case CodeStrength.VeryGood:
                pelengatingChance = 1;
                break;
            default:
                break;
        }
    }

    void StartPelengating(CodeSO code, SignalSO signal)
    {
        ChoosePelengatingChance(code);
        
        StartCoroutine(PelengatorRoutine());
    }

    void StartPelengating()
    {
        ChoosePelengatingChance();

        StartCoroutine(SendingSignalRoutine());
        StartCoroutine(PelengatorRoutine());
    }


    void CheckAndStartPelengation()
    {
        bool shouldBePelengating = isSending || enemyNearStation;

        if (shouldBePelengating && activePelengationRoutine == null)
        {
            activePelengationRoutine = StartCoroutine(Pelengating());
        }
    }

    void EnemyNearStation(bool flag)
    {
        enemyNearStation = flag;
        CheckAndStartPelengation();
    }

    IEnumerator Pelengating()
    {
        OnPelengatingStarted();
        while (isSending || enemyNearStation)
        {
            pelengator.fillAmount += 0.01f;
            OnPelengatorLevelWasChanged(pelengator.fillAmount);
            yield return new WaitForSeconds(pelengatingSpeed / kS * kN);
        }
        Debug.Log("2131");
        OnPelengatingFinished();
        activePelengationRoutine = null;
        StartCoroutine(DecreasingPelengatorBar());
    }

    IEnumerator DecreasingPelengatorBar()
    {
        while (!isPelengating && pelengator.fillAmount != 0 && pelengator.fillAmount != 1)
        {
            pelengator.fillAmount -= 0.01f;
            OnPelengatorLevelWasChanged(pelengator.fillAmount);
            yield return new WaitForSeconds(decreasingPelengatorSpeed);
        }
    }

    void SetNewSpeedKoef(float distance, float maxDistnce)
    {
        int koef = 1;
        if (distance < 0.01f)
            distance = 0.01f;
        koef = (int)(maxDistnce / distance);

        if (koef > 5) kS = 5;
        else if (koef < 1) kS = 1;
        else kS = koef;
    }

    void SetNewNoiseKoef(int koef)
    {
        kN = koef + 1;
    }

    IEnumerator PelengatorRoutine()
    {
        while (isSending)
        {
            yield return new WaitForSeconds(pelengatingTime);
            int chance = UnityEngine.Random.Range(0, 100);
            if (chance <= pelengatingChance && (pelengatingByEnemy == null && pelengatingBySignal == null))
            {
                CheckAndStartPelengation();
                break;
            }

        }
    }

    IEnumerator SendingSignalRoutine()
    {
        isSending = true;

        yield return new WaitForSeconds(sendingSignalTime);
        isSending = false;



    }

 
    private void OnEnable()
    {
        noiseGenerator.newNoiseLevelWasSet += SetNewNoiseKoef;
        enemyController.enemyNearStationEvent += SetNewSpeedKoef;
        enemyController.enemySpawned += EnemyNearStation;
        SignalsScreen.signalSent += SetNewCode;
        SignalsScreen.signalSent += StartPelengating;

    }

    private void OnDisable()
    {
        noiseGenerator.newNoiseLevelWasSet -= SetNewNoiseKoef;
        enemyController.enemyNearStationEvent -= SetNewSpeedKoef;
        enemyController.enemySpawned -= EnemyNearStation;
        SignalsScreen.signalSent -= SetNewCode;
        SignalsScreen.signalSent -= StartPelengating;
    }

}