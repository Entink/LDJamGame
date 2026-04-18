using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private InventoryUISlot slotPrefab;

    private void Start()
    {
        if(InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshUI;
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        Dictionary<string, int> items = InventoryManager.Instance.GetItems();

        foreach(KeyValuePair<string, int> pair in items)
        {
            InventoryItemData itemData = InventoryManager.Instance.GetItemData(pair.Key);
            InventoryUISlot slot = Instantiate(slotPrefab, contentParent);
            slot.Setup(itemData, pair.Value);
        }
    }
}
