using System;
using UnityEngine;
using Injection;
using System.Collections.Generic;

public class NoiseGenerator : MonoBehaviour
{
    [Inject] Injector injector;
    public Transform knob;            
    public Transform[] steps;         
    public MeshRenderer lamp;

    [SerializeField] List<NoiseGeneratorStep> noiseGeneratorSteps = new List<NoiseGeneratorStep>();


    [SerializeField] Material level0Mat;
    [SerializeField] Material level1Mat;
    [SerializeField] Material level2Mat;
    [SerializeField] Material level3Mat;
    [SerializeField] Material level4Mat;

    public float moveSpeed = 10f;
    public float baseK = 1f;

    int currentLevel = 0;
    int newLevel = 0;
    Vector3 velocity = Vector3.zero;

    public event Action<int> newNoiseLevelWasSet;
    
    private void OnNewNoiseLevelWasSet(int level)
    {
        newNoiseLevelWasSet?.Invoke(level);
    }


    public void Initialize()
    {
        injector.Inject(this);
        InitNoiseSteps();
        knob.position = steps[currentLevel].position;
        UpdateLamp();
    }

    void InitNoiseSteps()
    {
        foreach (var step in noiseGeneratorSteps)
        {
            injector.Inject(step);
        }
    }

    void Update()
    {
        Vector3 target = steps[currentLevel].position;

        knob.position = Vector3.SmoothDamp(
            knob.position,
            target,
            ref velocity,
            1f / moveSpeed
        );
    }


    public void SetLevel(int lvl)
    {
        newLevel = lvl;
        if (currentLevel == newLevel) return; 
        currentLevel = newLevel;
        OnNewNoiseLevelWasSet(currentLevel);
        UpdateLamp();
    }

    void UpdateLamp()
    {
        Material[] mats = lamp.materials;
        switch (currentLevel)
        {
            case 0: mats[1] = level0Mat; break;
            case 1: mats[1] = level1Mat; break;
            case 2: mats[1] = level2Mat; break;
            case 3: mats[1] = level3Mat; break;
            case 4: mats[1] = level4Mat; break;
        }
        lamp.materials = mats;
    }

    public float GetSlowdown()
    {
        if (currentLevel == 0)
            return 1f;              

        return baseK * currentLevel; 
    }
}


