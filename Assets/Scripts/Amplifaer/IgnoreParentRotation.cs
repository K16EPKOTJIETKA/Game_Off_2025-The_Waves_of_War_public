using UnityEngine;

public class IgnoreParentRotation : MonoBehaviour
{
    [SerializeField] private Quaternion targetWorldRotation = Quaternion.identity;

    void LateUpdate()
    {
        transform.rotation = targetWorldRotation;
    }
}
