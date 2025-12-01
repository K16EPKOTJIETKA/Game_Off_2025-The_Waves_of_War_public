using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    public float orthoSize = 5f;

    private Camera _cam;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _startIsOrtho;
    private float _startOrthoSize;
    private float _startFieldOfView;

    private void Awake()
    {
        _cam = GetComponent<Camera>();

        SaveOriginalState();
    }

    private void SaveOriginalState()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startIsOrtho = _cam.orthographic;
        _startOrthoSize = _cam.orthographicSize;
        _startFieldOfView = _cam.fieldOfView;
    }

    [ContextMenu("Switch To Ortho")]
    public void SwitchToOrtho(Transform targetPos)
    {
        if (_cam == null) return;

        transform.position = targetPos.position;
        transform.eulerAngles = targetPos.eulerAngles;

    }

    [ContextMenu("Reset To Original")]
    public void ResetCamera()
    {
        if (_cam == null) return;

        transform.position = _startPosition;
        transform.rotation = _startRotation;

        _cam.orthographic = _startIsOrtho;
        _cam.orthographicSize = _startOrthoSize;
        _cam.fieldOfView = _startFieldOfView;
    }
}
