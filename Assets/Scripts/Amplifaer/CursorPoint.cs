using UnityEngine;
using UnityEngine.InputSystem;

public class CursorPoint : MonoBehaviour
{
    public Camera mainCamera; 

    public LayerMask interactableLayer;

    private GameObject currentHoveredObject = null;

    [SerializeField] PlayerInteraction playerInteraction;
    public Texture2D defaultCursor;   
    public Texture2D interactCursor;
    public Vector2 defaultHotspot = Vector2.zero;
    public Vector2 interactHotspot = new Vector2(0,0);
    public void InitStart()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }
    }

    void Update()
    {
        if (!playerInteraction.isInteracting) return;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            Cursor.SetCursor(interactCursor, interactHotspot, CursorMode.Auto);
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentHoveredObject)
            {      
                currentHoveredObject = hitObject;

                if (hitObject.TryGetComponent(out IShowableUI showable))
                {
   
                    showable.ShowUI();
                }

               
                Debug.Log("Наведен на: " + hitObject.name);
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (hitObject.TryGetComponent(out IClickable clickable))
                {
                    clickable.OnClick();
                }
            }
            
        }
        else 
        {
            Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
            if (currentHoveredObject != null)
            {
    
                if (currentHoveredObject.TryGetComponent(out IShowableUI showable))
                {
                    showable.HideUI();
                }
                currentHoveredObject = null;
            }
        }
    }
}
