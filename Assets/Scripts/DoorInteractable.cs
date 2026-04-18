using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteract player)
    {
        Destroy(gameObject);
    }

    public string GetInteractionText(PlayerInteract player)
    {
        return "Open door [E]";
    }
}