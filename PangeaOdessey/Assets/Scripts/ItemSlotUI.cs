using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : BaseSlotUI
{
   public override void OnDrop(PointerEventData eventData)
   {
       if (eventData.pointerDrag == null) return;

       DraggableUI droppedItem = eventData.pointerDrag.GetComponent<DraggableUI>();

       if (droppedItem != null && droppedItem.item != null)
       {
           if (droppedItem.currentSlot is EquipmentSlotUI equipmentSlot)
           {
               EquipmentManager.Instance.UnequipItem(equipmentSlot.slotName);
               Debug.Log($"장비 해제 (인벤토리로 이동): {droppedItem.item.itemName}");
           }

           SwapItems(droppedItem);
       }
   }
}