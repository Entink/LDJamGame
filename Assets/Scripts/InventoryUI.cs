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

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
    }


    public void RefreshUI()
    {
        if (contentParent == null || slotPrefab == null)
            return;

        for(int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Transform child = contentParent.GetChild(i);
            if (child != null)
                Destroy(child.gameObject);
        }

        if (InventoryManager.Instance == null)
            return;


        Dictionary<string, int> items = InventoryManager.Instance.GetItems();

        foreach(KeyValuePair<string, int> pair in items)
        {
            InventoryItemData itemData = InventoryManager.Instance.GetItemData(pair.Key);
            if (itemData == null)
                continue;

            InventoryUISlot slot = Instantiate(slotPrefab, contentParent);
            slot.Setup(itemData, pair.Value);
        }
    }
}
