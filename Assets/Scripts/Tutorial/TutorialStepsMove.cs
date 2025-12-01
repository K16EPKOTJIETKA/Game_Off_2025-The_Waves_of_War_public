using Injection;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialStepsMove : MonoBehaviour
{
    [Inject] GameEventBuffer eventBuffer;
    [Inject] Injector injector;

    public InputActionReference interactAction;
    int step = 2;

    public void Initialize()
    {
        injector.Inject(this);
    }

    private void Update()
    {
        if (step > 7) return;
        if (interactAction.action.WasPressedThisFrame())
        {
            eventBuffer.CastNewEvent($"{step}");
            step++;
        }
    }
}
