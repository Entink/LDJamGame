using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int amount = 1;

    public void Interact(PlayerInteract player)
    {
        InventoryManager.Instance.AddItem(itemData, amount);
        Destroy(gameObject);
    }

    public string GetInteractionText(PlayerInteract player)
    {
        return "Pick up " + itemData.itemName + " [E]"; 
    }
}
