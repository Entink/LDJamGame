using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName ="Game/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemId;
    public string itemName;
    public Sprite icon;
}
