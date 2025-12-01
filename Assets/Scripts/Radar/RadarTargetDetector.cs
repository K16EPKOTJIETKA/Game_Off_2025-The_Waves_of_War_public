using UnityEngine;

public class RadarTargetDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        RadarTarget target = other.GetComponent<RadarTarget>();

        if (target != null)
        {
            target.LightUp();
        }
    }
}