using UnityEngine;
using Unity.Cinemachine;

public class CustomFPSCameraRotator : MonoBehaviour
{
    [Header("Cinemachine Pan Tilt Component")]
    [SerializeField] private CinemachinePanTilt panTiltComponent;
    
    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    
    [Header("Invert Controls")]
    [SerializeField] private bool invertPan = false;
    [SerializeField] private bool invertTilt = false;
    
    private void Start()
    {
        // If no component is assigned, try to find it on the same GameObject
        if (panTiltComponent == null)
        {
            panTiltComponent = GetComponent<CinemachinePanTilt>();
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
        
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Apply inversion if needed
        if (invertPan) mouseX = -mouseX;
        if (invertTilt) mouseY = -mouseY;
        
        // Update Pan Axis (horizontal rotation)
        panTiltComponent.PanAxis.Value += mouseX * Time.deltaTime;
        panTiltComponent.PanAxis.Validate();
        // Update Tilt Axis (vertical rotation)
        panTiltComponent.TiltAxis.Value += mouseY * Time.deltaTime; 
        panTiltComponent.TiltAxis.Validate();
    }
}
