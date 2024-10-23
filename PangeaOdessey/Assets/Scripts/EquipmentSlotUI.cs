using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentSlotUI : BaseSlotUI
{
    public string slotName;
    public EquipmentType acceptedEquipmentType;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text speedText;

    public override void OnDrop(PointerEventData eventData)
    {
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
    
        if (droppedItem != null && droppedItem.item != null)
        {
            if (droppedItem.item.equipmentType == acceptedEquipmentType)
            {
                SwapItems(droppedItem);
                EquipmentManager.Instance.EquipItem(slotName, droppedItem.item);
                UpdateStatPanel(droppedItem.item);
                Debug.Log($"장비 장착: {droppedItem.item.itemName}");
            }
            else
            {
                Debug.Log("이 슬롯에는 해당 장비를 장착할 수 없습니다.");
                droppedItem.ReturnToOriginalSlot();
            }
        }
    }

    public void UpdateStatPanel(Item item)
    {
        var totalStats = EquipmentManager.Instance.GetTotalStats();
        nameText.text = item != null ? $"{item.itemName}" : "Empty Slot";
        healthText.text = $"Total HP: {totalStats.hp}";
        attackText.text = $"Total Attack: {totalStats.damage}%";
        speedText.text = $"Total Speed: {totalStats.speed}%";
    }

    public void ClearStatPanel()
    {
        UpdateStatPanel(null);
    }

    protected override void RemoveItem()
    {
        base.RemoveItem();
        EquipmentManager.Instance.UnequipItem(slotName);
        UpdateStatPanel(null);
    }
}