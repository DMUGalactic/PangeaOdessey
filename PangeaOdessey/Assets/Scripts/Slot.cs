using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : BaseSlotUI
{
    [SerializeField] private new Image image; // 슬롯의 이미지를 표시할 UI 이미지 컴포넌트
    private Item _item; // 슬롯에 장착된 아이템을 저장하는 변수

    // 슬롯에 장착된 아이템의 프로퍼티
    public Item item
    {
        get { return _item; } // 아이템을 반환하는 getter
        set
        {
            _item = value; // 아이템을 설정
            if (_item != null) // 아이템이 null이 아닐 경우
            {
                image.sprite = item.itemImage; // 아이템의 이미지를 슬롯 이미지에 설정
                image.color = new Color(1, 1, 1, 1); // 슬롯 이미지를 보이도록 설정 (불투명)
            }
            else // 아이템이 null인 경우
            {
                image.color = new Color(1, 1, 1, 0); // 슬롯 이미지를 숨김 (투명)
            }
        }
    }

    // 드래그 가능한 아이템을 슬롯에 설정하는 메서드
    public void SetDraggableItem(DraggableUI draggable)
    {
        if (draggable != null && item != null) // 드래그 가능한 아이템과 슬롯에 아이템이 있는지 확인
        {
            draggable.SetItem(item); // 드래그 가능한 UI에 현재 슬롯의 아이템 설정
        }
    }

    // 드롭된 아이템을 처리하는 메서드
    public override void OnDrop(PointerEventData eventData)
    {
        // 드롭된 아이템의 DraggableUI 컴포넌트를 가져옴
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
        if (droppedItem != null) // 드롭된 아이템이 유효한 경우
        {
            SwapItems(droppedItem); // 현재 슬롯의 아이템과 드롭된 아이템을 교환
        }
    }
}
