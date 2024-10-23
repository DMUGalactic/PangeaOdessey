using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentSlotUI : BaseSlotUI
{
    public int slotID;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text speedText;
    private void Start()
{
    LoadEquippedItem(); // 장착된 아이템 로드
}

private void LoadEquippedItem()
{
    Item item = EquipmentManager.Instance.GetEquippedItem(slotID);
    if (item != null)
    {
        UpdateStatPanel(item);
    }
    else
    {
        ClearStatPanel();
    }
}

    public override void OnDrop(PointerEventData eventData)
    {
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
    
        if (droppedItem != null && droppedItem.item != null)
        {
            if (droppedItem.item.itemID == slotID)
            {
                SwapItems(droppedItem);
                EquipmentManager.Instance.EquipItem(slotID, droppedItem.item);
                UpdateStatPanel(droppedItem.item);
                Debug.Log($"장비 장착: 아이템 ID {droppedItem.item.itemID}");
            }
            else
            {
                Debug.Log($"이 슬롯에는 아이템 ID {droppedItem.item.itemID}를 장착할 수 없습니다. 필요한 ID: {slotID}");
                droppedItem.ReturnToOriginalSlot();
            }
        }
    }

    public void UpdateStatPanel(Item item)
{
    var totalStats = EquipmentManager.Instance.GetTotalStats(); // 총 스탯을 항상 가져오기
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
        EquipmentManager.Instance.UnequipItem(slotID);
        UpdateStatPanel(null);
    }
}