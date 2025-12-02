using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CustomFPSCameraRotator : MonoBehaviour
{
    [Header("Cinemachine Pan Tilt Component")]
    [SerializeField] private CinemachinePanTilt panTiltComponent;

    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.0f;

    [Header("Invert Controls")]
    [SerializeField] private bool invertPan = false;
    [SerializeField] private bool invertTilt = false;

    [Header("Smoothing")]
    [SerializeField] private float smoothingTime = 0.08f;

    private InputAction mouseMoveAction;

    // Smoothing variables
    private float currentPanVelocity;
    private float currentTiltVelocity;
    private float targetPanDelta;
    private float targetTiltDelta;

    private void Start()
    {
        // If no component is assigned, try to find it on the same GameObject
        if (panTiltComponent == null)
        {
            panTiltComponent = GetComponent<CinemachinePanTilt>();
        }

        // Initialize Input System action
        mouseMoveAction = InputSystem.actions.FindAction("Look");
        if (mouseMoveAction == null)
        {
            Debug.LogWarning("Input Action 'Look' not found! Make sure you have an Input Action Asset with a 'Look' action.");
        }
    }

    private void Update()
    {
        // Check if component exists
        if (panTiltComponent == null)
        {
            Debug.LogWarning("CinemachinePanTilt component not found!");
            return;
        }

        // Check if input action exists
        if (mouseMoveAction == null)
        {
            return;
        }

        // Get mouse input from new Input System
        Vector2 mouseDelta = mouseMoveAction.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        // Apply inversion if needed
        if (invertPan) mouseX = -mouseX;
        if (invertTilt) mouseY = -mouseY;

        // Calculate target delta
        targetPanDelta = mouseX * Time.deltaTime;
        targetTiltDelta = mouseY * Time.deltaTime;

        // Smooth Pan Axis (horizontal rotation)
        float newPan = Mathf.SmoothDamp(
            panTiltComponent.PanAxis.Value,
            panTiltComponent.PanAxis.Value + targetPanDelta,
            ref currentPanVelocity,
            smoothingTime
        );
        panTiltComponent.PanAxis.Value = newPan;
        panTiltComponent.PanAxis.Validate();

        // Smooth Tilt Axis (vertical rotation)
        float newTilt = Mathf.SmoothDamp(
            panTiltComponent.TiltAxis.Value,
            panTiltComponent.TiltAxis.Value + targetTiltDelta,
            ref currentTiltVelocity,
            smoothingTime
        );
        panTiltComponent.TiltAxis.Value = newTilt;
        panTiltComponent.TiltAxis.Validate();
    }
}