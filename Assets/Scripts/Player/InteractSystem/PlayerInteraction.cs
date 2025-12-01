using TMPro; 
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;
    public float interactionDistance = 10f;

    public GameObject interactionUI;
    public GameObject point;
    public TextMeshProUGUI interactionText;
    public InputActionReference interactAction;
    public bool isInteracting;
    public float ignoreInputUntil = 0f;
    void Update()
    {
        InteractionRay();
    }

    void InteractionRay()
    {
        if (isInteracting)
        {
            point.SetActive(false);
            return;
        }

        if (Time.time < ignoreInputUntil)
            return;

        point.SetActive(true);
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;

        bool hitSomething = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                hitSomething = true;
                
                interactionText.text = interactable.GetDescription();

                if (interactAction.action.WasPressedThisFrame())
                {
                    interactable.Interact();
                    point.SetActive(false);
                    interactionText.text = string.Empty;
                }
            }
        }

        interactionUI.SetActive(hitSomething);
    }
}
