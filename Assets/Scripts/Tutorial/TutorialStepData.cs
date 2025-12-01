using UnityEngine;

[System.Serializable]
public class TutorialStepData
{
    [TextArea]
    public string tutorialContentText;
    public string tutorialTriggerEventKey;
    public Transform tutorialIndicatorTargetTransform;
    public Vector3 tutorialIndicatorPositionOffset;
    public Vector3 lookAtOrientationVector = new Vector3(0, 0, -1);
}
