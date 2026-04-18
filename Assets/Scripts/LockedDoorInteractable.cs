using UnityEngine;

public class LockedDoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool consumeItem;
    [SerializeField] private string missingItemMessage = "You need a key.";

    public void Interact(PlayerInteract player)
    {
        if (player.HasItem(requiredItemId))
        {
            if (consumeItem)
                player.RemoveItem(requiredItemId, 1);

            Destroy(gameObject);
        }
        else
        {
            InteractionMessageUI.Instance?.ShowMessage(missingItemMessage);
        }
    }

    public string GetInteractionText(PlayerInteract player)
    {
        if (player.HasItem(requiredItemId))
            return "Unlock door [E]";

        return "Locked";
    }
}