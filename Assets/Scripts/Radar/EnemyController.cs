using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Injection;

public class EnemyController : MonoBehaviour
{
    [Inject] PelengatorController pelengatorController;
    [Inject] Injector injector;

    [SerializeField] List<GameObject> enemyGroups = new List<GameObject>();
    [SerializeField] Transform radarPos;
    [SerializeField] Transform pointOnRadarCircle;

    [SerializeField] int spawnEnemyChance = 30;
    [SerializeField] int radarTime = 60; 
    [SerializeField] int dangerKoef = 10;
    float radius;
    int currentEnemyGroup = 0;
    int danger = 0;
    float enemySpeed = 0.5f;

    bool enemyIsMoving = false;

    public event Action<float, float> enemyNearStationEvent;
    public event Action<bool> enemySpawned;

    void ChangeDanger(float pelengatorLvl)
    {
        int lvl = (int)(pelengatorLvl * 100);
        danger = lvl / 25;
    }

    void OnEnemyNearStationEvent(float distance, float maxDistance)
    {
        enemyNearStationEvent(distance, maxDistance);
    }

    void OnEnemySpawned(bool flag)
    {
        enemySpawned?.Invoke(flag);
    }

    public void Initialize()
    {
        injector.Inject(this);

        radius = Vector3.Distance(radarPos.position, pointOnRadarCircle.position);
        StartCoroutine(TryingSpawnEnemy());
    }

    Vector3 ChoosePosOnRing()
    {

        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle.normalized * radius;
        return new Vector3(radarPos.position.x + randomPoint.x, radarPos.position.y + randomPoint.y, enemyGroups[0].transform.position.z);
    }

    Vector3 ChoosePosOnCircle()
    {

        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * radius;
        return new Vector3(radarPos.position.x + randomPoint.x, radarPos.position.y + randomPoint.y, enemyGroups[0].transform.position.z);
    }


    IEnumerator TryingSpawnEnemy()
    {
        while (true)
        {
            int chance = UnityEngine.Random.Range(0, 100);
            yield return new WaitForSeconds(radarTime - (danger * dangerKoef));
            if (chance <= spawnEnemyChance && !enemyIsMoving)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        enemyIsMoving = true;
        List<Vector3> intermediatePositions = new List<Vector3>();
        Vector3 startPos = ChoosePosOnRing();
        currentEnemyGroup = UnityEngine.Random.Range(0, 1 + danger < enemyGroups.Count ? 1 + danger : enemyGroups.Count - 1);
        int countOfIntermediatePos = UnityEngine.Random.Range(0, 2 + danger);
        for (int i = 0; i <= danger; i++)
        {
            if (i == danger)
                intermediatePositions.Add(ChoosePosOnRing());
            else
            {
                intermediatePositions.Add(ChoosePosOnCircle());
            }
        }

        enemyGroups[currentEnemyGroup].transform.position = startPos;
        enemyGroups[currentEnemyGroup].SetActive(true);
        StartCoroutine(MoveEnemy(intermediatePositions));

    }

    Vector3 endPos;
    void SpawnLastEnemy()
    {
        enemyIsMoving = true;
        Vector3 startPos = ChoosePosOnRing();
        currentEnemyGroup = 5;
        int countOfIntermediatePos = UnityEngine.Random.Range(0, 2 + danger);
        endPos = new Vector3(radarPos.position.x, radarPos.position.y, enemyGroups[0].transform.position.z);
        enemyGroups[currentEnemyGroup].transform.position = startPos;
        enemyGroups[currentEnemyGroup].SetActive(true);
        StartCoroutine(MoveLastEnemy());
    }

    IEnumerator MoveLastEnemy()
    {

        Transform enemyTransform = enemyGroups[currentEnemyGroup].transform;


        Vector3 target = endPos;

        while (Vector3.Distance(enemyTransform.position, target) > 0.001f)
        {
            enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, target, enemySpeed * Time.deltaTime);
            yield return null;
        }

        enemyTransform.position = target;


        enemyIsMoving = false;
        yield return new WaitForSeconds(5);
        LastEnemyOnBase();
    }

    void LastEnemyOnBase()
    {
        pelengatorController.OnPlayerDeadEvent();
    }

    IEnumerator MoveEnemy(List<Vector3> intermediatePositions)
    {
        OnEnemySpawned(true);
        int currentPoint = 0;
        Transform enemyTransform = enemyGroups[currentEnemyGroup].transform;

        while (currentPoint < intermediatePositions.Count)
        {
            Vector3 target = intermediatePositions[currentPoint];

            while (Vector3.Distance(enemyTransform.position, target) > 0.001f)
            {
                enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, target, enemySpeed * Time.deltaTime);
                OnEnemyNearStationEvent(Vector3.Distance(enemyTransform.position, radarPos.position), radius);
                yield return null;
            }

            enemyTransform.position = target;

            currentPoint++;
        }
        intermediatePositions.Clear();
        OnEnemySpawned(false);
        enemyGroups[currentEnemyGroup].SetActive(false);
        enemyIsMoving = false;

    }

    private void OnEnable()
    {
        pelengatorController.pelengatorLevelWasChanged += ChangeDanger;
        pelengatorController.playerDeathStarted += SpawnLastEnemy;
    }

    private void OnDisable()
    {
        pelengatorController.pelengatorLevelWasChanged -= ChangeDanger;
        pelengatorController.playerDeathStarted -= SpawnLastEnemy;
    }
}
