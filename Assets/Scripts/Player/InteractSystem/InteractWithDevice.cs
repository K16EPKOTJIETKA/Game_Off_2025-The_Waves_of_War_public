using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithDevice : MonoBehaviour, IInteractable
{
    [SerializeField] Transform cameraPos;
    [SerializeField] CinemachineCamera normalCamera;  
    [SerializeField] CinemachineCamera mapCamera;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerController playerController;
    public InputActionReference interactAction;
    [SerializeField] BoxCollider boxCollider;

    private float interactionCooldown = 0f;
    private bool isInDeviceMode = false;
 
    public string GetDescription()
    {
        return "Press [E]";
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        if (playerInteraction.isInteracting) return;
        SwitchToDeviceCamera();
        
    }

    public void SetColliderState(bool state)
    {
        boxCollider.enabled = state;
    }
    private void Update()
    {
        if (!playerInteraction.isInteracting) return;
        if (Time.time < interactionCooldown) return;
        if (interactAction.action.WasPressedThisFrame())
        {
            ReturnToNormalCamera();
        }
    }

    private void SwitchToDeviceCamera()
    {
        mapCamera.transform.position = cameraPos.position;
        mapCamera.transform.rotation = cameraPos.rotation;
        playerInteraction.enabled = false;
        playerController.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isInDeviceMode = true;
        playerInteraction.isInteracting = true;

        mapCamera.Priority = 20;
        normalCamera.Priority = 1;
        interactionCooldown = Time.time + 0.2f;
        Debug.Log("Switched to device camera");
    }

    private void ReturnToNormalCamera()
    {
        mapCamera.Priority = 1;
        normalCamera.Priority = 20;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerInteraction.ignoreInputUntil = Time.time + 0.2f;

        isInDeviceMode = false;
        Debug.Log("Before exit: playerInteraction.isInteracting = " + playerInteraction.isInteracting);
        playerInteraction.isInteracting = false;
        playerInteraction.enabled = true;
        playerController.enabled = true;
        Debug.Log("After exit: playerInteraction.isInteracting = " + playerInteraction.isInteracting);

        Debug.Log("Returned to normal camera");
    }

    IEnumerator OffCamera()
    {
        yield return new WaitForSeconds(0.2f);
        mapCamera.Priority = 1;
        normalCamera.Priority = 20;
    }
}
