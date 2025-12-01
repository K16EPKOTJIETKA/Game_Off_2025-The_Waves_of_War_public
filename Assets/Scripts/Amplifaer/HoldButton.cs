using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Настройки")]
    [Tooltip("Time (in sec), after which a press is considered 'long'")]
    public float initialDelay = 0.5f;

    [Tooltip("The frequency (in sec) at which a 'long' press is triggered")]
    public float repeatRate = 0.01f;

    [Header("Events")]
    [Tooltip("Event for one click")]
    public UnityEvent onClick;

    [Tooltip("Event for hold")]
    public UnityEvent onHoldRepeat;

    private bool isPointerDown = false;
    private bool isHoldMode = false;
    private float timePressed = 0f;
    private float timeOfNextTick = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        isHoldMode = false;
        timePressed = Time.time; 

        timeOfNextTick = Time.time + initialDelay;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPointerDown && !isHoldMode)
        {
            if (onClick != null)
            {
                onClick.Invoke();
            }
        }

        isPointerDown = false;
        isHoldMode = false;
    }

    void Update()
    {
        if (!isPointerDown)
        {
            return; 
        }

        if (Time.time > timeOfNextTick)
        {
            if (!isHoldMode)
            {
                isHoldMode = true;
            }

            if (onHoldRepeat != null)
            {
                onHoldRepeat.Invoke();
            }

            timeOfNextTick = Time.time + repeatRate;
        }
    }
}
