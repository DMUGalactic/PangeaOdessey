using UnityEngine;
using UnityEngine.EventSystems;

// EquipmentSlotUI: 장비 슬롯을 관리하는 클래스입니다.
public class EquipmentSlotUI : BaseSlotUI
{
    // 이 슬롯에 맞는 장비 타입을 지정합니다 ("Head", "Body", "Gloves", "Shoes")
    public string equipmentType;

    // OnDrop: 아이템이 이 슬롯에 드롭될 때 호출되는 메서드입니다.
    public override void OnDrop(PointerEventData eventData)
    {
        // 드롭된 오브젝트에서 DraggableUI 컴포넌트를 가져옵니다.
        DraggableUI droppedItem = eventData.pointerDrag.GetComponent<DraggableUI>();
        
        // 드롭된 아이템이 유효하고, 이 슬롯의 장비 타입과 일치하는 경우 처리를 진행합니다.
        if (droppedItem != null && droppedItem.itemName == equipmentType)
        {
            // 현재 슬롯에 이미 아이템이 있다면 해제합니다.
            if (currentItem != null)
            {
                InventoryManager.Instance.UnequipItem(currentItem.itemName);
            }
            
            // 아이템을 교환합니다.
            SwapItems(droppedItem);
            
            // 새로운 아이템을 장착합니다.
            InventoryManager.Instance.EquipItem(droppedItem.itemName);
        }
        else
        {
            // 잘못된 장비 타입이면 원래 위치로 돌려보냅니다.
            droppedItem.GetComponent<DraggableUI>().ReturnToOriginalSlot();
        }
    }

    // RemoveItem: 아이템을 슬롯에서 제거하는 메서드입니다.
    protected override void RemoveItem()
    {
        // 현재 아이템이 있다면 장비를 해제합니다.
        if (currentItem != null)
        {
            InventoryManager.Instance.UnequipItem(currentItem.itemName);
        }
        // 기본 RemoveItem 메서드를 호출합니다.
        base.RemoveItem();
    }
}