using UnityEngine.EventSystems;

// ItemSlotUI: 인벤토리의 아이템 슬롯을 관리하는 클래스입니다.
public class ItemSlotUI : BaseSlotUI
{
    // OnDrop: 아이템이 이 슬롯에 드롭될 때 호출되는 메서드입니다.
    public override void OnDrop(PointerEventData eventData)
    {
        // 드롭된 오브젝트에서 DraggableUI 컴포넌트를 가져옵니다.
        DraggableUI droppedItem = eventData.pointerDrag.GetComponent<DraggableUI>();
        
        // 드롭된 아이템이 유효한 경우 처리를 진행합니다.
        if (droppedItem != null)
        {
            // 드롭된 아이템이 장비 슬롯에서 왔는지 확인합니다.
            if (droppedItem.currentSlot is EquipmentSlotUI)
            {
                // 장비 슬롯에서 왔다면, 해당 아이템을 장비 해제합니다.
                InventoryManager.Instance.UnequipItem(droppedItem.itemName);
            }
            
            // 아이템을 현재 슬롯과 교환합니다.
            SwapItems(droppedItem);
        }
    }
}