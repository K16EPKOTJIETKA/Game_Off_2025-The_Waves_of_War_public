using UnityEngine;
using Injection;

public class MessageBootstrap : MonoBehaviour
{
    [SerializeField] MessageSystem messageSystem;
    [SerializeField] MessageScreen messageScreen;
    [Inject] Injector injector;
    public void Initialize()
    {
        injector.Inject(messageSystem);

        messageScreen.Init();
        messageSystem.Init();
    }
}
