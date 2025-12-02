using UnityEngine;
using UnityEngine.InputSystem;

public class RawMouseInput : MonoBehaviour
{
    public Transform cameraRoot;

    public float sensitivityX = 0.1f; 
    public float sensitivityY = 0.1f; 
    public float yClampMin = -80f;    
    public float yClampMax = 80f;    

    public float smoothTime = 0.02f;
    public float maxMouseDelta = 100f;
    private float _xRotation = 0f; 
    private Vector2 _currentMouseDelta;
    private Vector2 _currentMouseDeltaVelocity;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (Mouse.current == null) return;
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 targetMouseDelta = Mouse.current.delta.ReadValue();

        targetMouseDelta = Vector2.ClampMagnitude(targetMouseDelta, maxMouseDelta);

        _currentMouseDelta = Vector2.SmoothDamp(
            _currentMouseDelta,
            targetMouseDelta,
            ref _currentMouseDeltaVelocity,
            smoothTime
        );

        _currentMouseDelta = Vector2.ClampMagnitude(_currentMouseDelta, maxMouseDelta);

        float mouseX = _currentMouseDelta.x * sensitivityX;
        float mouseY = _currentMouseDelta.y * sensitivityY;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, yClampMin, yClampMax);
        cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) Cursor.lockState = CursorLockMode.Locked;
    }
}
