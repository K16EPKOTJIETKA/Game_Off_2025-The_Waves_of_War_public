using UnityEngine;

public class Object : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int randomNumber = Random.Range(0, 101);
        Debug.Log($"Random number: {randomNumber}");
    }

    public string GetDescription()
    {
        return "Press [E]";
    }
}