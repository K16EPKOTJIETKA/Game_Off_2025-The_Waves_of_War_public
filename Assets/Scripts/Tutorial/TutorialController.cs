using Injection;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//tO LAUNCH IN tutorial scene - press Initialze button
//then press OnNewEvent button with argument setted in 
//TutorialStepData.tutorialTriggerEventKey to advance to the next step

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialStepData> tutorialData;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject tutorialIndicator;
    [Inject] private GameEventBuffer gameEventBuffer;
    [Inject] Injector injector;
    private int currentStepIndex = 0;
    
    [EasyButtons.Button]
    public void Initialize()
    {
        injector.Inject(this);
        currentStepIndex = 0;
        ApplyNewTutorialStep(tutorialData[currentStepIndex]);
        tutorialIndicator.SetActive(true);
    }
    
    [EasyButtons.Button]
    private void OnNewEvent(string eventName)
    {
        if (eventName == tutorialData[currentStepIndex].tutorialTriggerEventKey)
        {
            currentStepIndex++;
            if (currentStepIndex >= tutorialData.Count)
            {
                tutorialIndicator.SetActive(false);
                return;
            }
            ApplyNewTutorialStep(tutorialData[currentStepIndex]);
        }
    }

    private void ApplyNewTutorialStep(TutorialStepData tutorialStepData)
    {
        tutorialText.text = tutorialStepData.tutorialContentText;
        tutorialIndicator.transform.position = tutorialStepData.tutorialIndicatorTargetTransform.position + tutorialStepData.tutorialIndicatorPositionOffset;
        tutorialIndicator.transform.LookAt(tutorialStepData.tutorialIndicatorTargetTransform.position + tutorialStepData.lookAtOrientationVector);
    }

    private void OnEnable()
    {
        gameEventBuffer.OnNewEvent += OnNewEvent;
    }

    private void OnDisable()
    {
        gameEventBuffer.OnNewEvent -= OnNewEvent;
    }
}
