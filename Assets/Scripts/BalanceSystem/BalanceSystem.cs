using Injection;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BalanceSystem : MonoBehaviour
{
    [Header("Base Settings")]
    public int totalPoints = 100;
    public int allyPoints;
    public int enemyPoints;

    [Header("Enemy Pressure")]
    public float tFine = 5f;
    public int minN = 1;
    public int maxN = 4;

    private float timer;

    [Header("Final Battle")]
    public bool finalBattle = false;
    public float multiplier = 3f;

    public event Action<float> OnBalanceChanged;
    public event Action<int, int> OnPointsChanged;
    public event Action<int, bool> phaseEndingWasGot;
    public event Action<int> gameEndingWasGot;

    int currentPhase = 0;
    int currentScore = 0;
    float needScore = 0;
    

    [SerializeField] BalanceBarUI balanceBarUI;
    [SerializeField] ScoreUI allyScore;
    [SerializeField] ScoreUI enemyScore;
    [Inject] SignalManager signalManager;
    [Inject] Injector injector;


    List<bool> phaseResults = new List<bool>();
    [SerializeField] float[] goodPercent = new float[5];
    [SerializeField] float[] badPercent = new float[5];
    

    public void Initialize()
    {
        injector.Inject(this);
        injector.Inject(balanceBarUI);
        injector.Inject(allyScore);
        injector.Inject(enemyScore);

        balanceBarUI.Init();
        allyScore.Init();
        enemyScore.Init();



        allyPoints = totalPoints / 2;
        enemyPoints = totalPoints / 2;

        SendUpdate();
        GetNeedScore();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tFine)
        {
            timer = 0;

            int gain = UnityEngine.Random.Range(minN, maxN + 1);
            AddPointsToEnemy(gain);
        }
    }

    public void StartFinalBattle()
    {
        if (finalBattle) return;
        finalBattle = true;

        tFine /= multiplier;
        minN = Mathf.RoundToInt(minN * multiplier);
        maxN = Mathf.RoundToInt(maxN * multiplier);
    }

    public string GetWinner()
    {
        if (allyPoints > totalPoints / 2) return "The Allies won";
        if (enemyPoints > totalPoints / 2) return "The enemies won";
        return "Draw";
    }

    public void AddPointsToAlly(int v)
    {
        allyPoints += v;
        currentScore += v;
        enemyPoints -= v;
        ClampValues();
        SendUpdate();
    }

    void AddPointsToEnemy(int v)
    {
        enemyPoints += v;
        allyPoints -= v;
        ClampValues();
        SendUpdate();
    }

    void ClampValues()
    {
        allyPoints = Mathf.Clamp(allyPoints, 0, totalPoints);
        enemyPoints = Mathf.Clamp(enemyPoints, 0, totalPoints);
    }

    void SendUpdate()
    {
        float percent = allyPoints / (float)totalPoints;

        OnBalanceChanged?.Invoke(percent);
        OnPointsChanged?.Invoke(allyPoints, enemyPoints);
    }

    void OnGameEndingWasGot(int ending)
    {
        gameEndingWasGot?.Invoke(ending);
    }

    void GetGameEnding()
    {
        if (phaseResults[4])
        {
            int goodPhaseEndings = 0;
            foreach (var phase in phaseResults)
            {
                if(phase) goodPhaseEndings++;
            }
            if (goodPhaseEndings <= 3)
            {
                OnGameEndingWasGot(2);
            }
            else OnGameEndingWasGot(0);
        }
        else
        {
            OnGameEndingWasGot(1);
        }
    }

    void GoToNextPhase(int phase)
    {
        GetPhaseEnding(); 
        currentPhase = phase;
        GetNeedScore();
        currentScore = 0;
        if (phase > 4)
        {
            GetGameEnding();
        }

    }   

    void GetNeedScore()
    {

        if (currentPhase == 0)
        {
            needScore = (goodPercent[currentPhase] / 100) * signalManager.maxPhaseScore[currentPhase];
            return;
        }

        if (phaseResults[currentPhase - 1])
        {
            needScore = (goodPercent[currentPhase] / 100) * signalManager.maxPhaseScore[currentPhase];
        }
        else
        {
            needScore = (badPercent[currentPhase] / 100) * signalManager.maxPhaseScore[currentPhase];
        }
    }
    
    void GetPhaseEnding()
    {
        phaseResults.Add(currentScore >= needScore);
        phaseEndingWasGot?.Invoke(currentPhase, currentScore >= needScore);
    }

    private void OnEnable()
    {
        signalManager.phaseChanged += GoToNextPhase;
    }
    private void OnDisable()
    {
        signalManager.phaseChanged -= GoToNextPhase;
    }
}

