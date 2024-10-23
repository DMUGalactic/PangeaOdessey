using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : BaseSlotUI
{
    [SerializeField] private new Image image;
    private Item _item;
    public EquipmentDB equipmentdb;  // EquipmentDB를 통해 데이터를 가져옴
    public int slotIndex; // 이 슬롯의 인덱스 (EquipmentDB에서의 슬롯 위치)

    public Item item
    {
        get
        {
            return _item;
        }
        set
        {
            // EquipmentDB에서 아이템 정보 반영
            if (equipmentdb != null)
            {
                int itemId = equipmentdb.inventory[slotIndex]; // Inventory 배열에서 아이템 ID 가져오기
                _item = equipmentdb.GetItemById(itemId); // EquipmentDB에서 직접 아이템 가져오기

                if (_item != null)
                {
                    image.sprite = _item.itemImage; // 아이템 이미지 설정
                    image.color = new Color(1, 1, 1, 1); // 아이템이 있으면 이미지 표시
                }
                else
                {
                    image.color = new Color(1, 1, 1, 0); // 아이템이 없으면 이미지 숨김
                }
            }
        }
    }

    public void SetDraggableItem(DraggableUI draggable)
    {
        if (draggable != null && item != null)
        {
            draggable.SetItem(item); // 드래그 가능한 UI에 아이템 설정
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
        if (droppedItem != null)
        {
            SwapItems(droppedItem); // 아이템 교체 로직
        }
    }
}
