using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(InventoryItemData itemData, int amount)
    {
        iconImage.sprite = itemData.icon;
        amountText.text = amount.ToString();
    }
}
