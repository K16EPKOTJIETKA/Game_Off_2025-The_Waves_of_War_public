using UnityEngine;

[CreateAssetMenu(fileName = "CodeSO", menuName = "Scriptable Objects/CodeSO")]
public class CodeSO : ScriptableObject
{
    public string codeName;
    public float codingTime;
    public CodeStrength codeStrength;
}

public enum CodeStrength
{
    NoStrength = 1,
    Bad,
    Medium,
    Good,
    VeryGood,
}
