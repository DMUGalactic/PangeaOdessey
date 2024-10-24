using UnityEngine;
using UnityEngine.EventSystems;

// 아이템 슬롯 UI 클래스. 기본 슬롯 UI를 상속받아 드랍 이벤트를 처리함.
public class ItemSlotUI : BaseSlotUI
{
    // 드랍 이벤트 처리 메서드
    public override void OnDrop(PointerEventData eventData)
    {
        // 드랍된 아이템이 없으면 종료
        if (eventData.pointerDrag == null) return;

        // 드랍된 아이템의 DraggableUI 컴포넌트를 가져옴
        DraggableUI droppedItem = eventData.pointerDrag.GetComponent<DraggableUI>();

        // 드랍된 아이템이 유효한지 확인
        if (droppedItem != null && droppedItem.item != null)
        {
            // 드랍된 아이템의 현재 슬롯이 EquipmentSlotUI인 경우
            if (droppedItem.currentSlot is EquipmentSlotUI equipmentSlot)
            {
                // 해당 장비를 해제하고 인벤토리로 이동
                EquipmentManager.Instance.UnequipItem(equipmentSlot.slotName);
                Debug.Log($"장비 해제 (인벤토리로 이동): {droppedItem.item.itemName}");
            }

            // 현재 슬롯에 드랍된 아이템을 추가
            SwapItems(droppedItem);
        }
    }
}
