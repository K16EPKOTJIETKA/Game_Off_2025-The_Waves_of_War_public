using UnityEngine;

[CreateAssetMenu(fileName = "MessageSO", menuName = "Scriptable Objects/MessageSO")]
public class MessageSO : ScriptableObject
{
    public int id;
    public int signalId;
    [TextArea]
    public string message;
}
