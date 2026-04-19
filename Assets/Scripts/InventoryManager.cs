using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> items = new Dictionary<string, int>();
    private Dictionary<string, InventoryItemData> itemDataLookup = new Dictionary<string, InventoryItemData>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(InventoryItemData itemData, int amount = 1)
    {
        if (itemData == null)
        {
            Debug.LogError("InventoryManager.AddItem: itemData is NULL");
            return;
        }

        if (string.IsNullOrEmpty(itemData.itemId))
        {
            Debug.LogError("InventoryManager.AddItem: itemData.itemId is empty", itemData);
            return;
        }

        if (items.ContainsKey(itemData.itemId))
            items[itemData.itemId] += amount;
        else
            items[itemData.itemId] = amount;

        if (!itemDataLookup.ContainsKey(itemData.itemId))
            itemDataLookup.Add(itemData.itemId, itemData);
        else
            itemDataLookup[itemData.itemId] = itemData;

        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(string itemId, int amount = 1)
    {
        return items.ContainsKey(itemId) && items[itemId] >= amount;
    }

    public bool RemoveItem(string itemId, int amount = 1)
    {
        if (!HasItem(itemId, amount))
            return false;

        items[itemId] -= amount;

        if (items[itemId] <= 0)
            items.Remove(itemId);

        OnInventoryChanged?.Invoke();
        return true;
    }

    public Dictionary<string, int> GetItems()
    {
        return items;
    }

    public InventoryItemData GetItemData(string itemId)
    {
        if (itemDataLookup.TryGetValue(itemId, out InventoryItemData data))
            return data;

        return null;
    }

    public void ClearInventory()
    {
        items.Clear();
        itemDataLookup.Clear();
        OnInventoryChanged?.Invoke();
    }
}