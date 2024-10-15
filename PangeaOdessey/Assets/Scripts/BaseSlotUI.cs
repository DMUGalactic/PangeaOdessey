using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// BaseSlotUI: 모든 슬롯 UI의 기본 클래스입니다.
public abstract class BaseSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Image image;          // 슬롯의 이미지 컴포넌트
    public DraggableUI currentItem; // 현재 슬롯에 있는 아이템

    // Awake: 컴포넌트 초기화
    protected virtual void Awake()
    {
        image = GetComponent<Image>(); // 이미지 컴포넌트 가져오기
    }

    // OnDrop: 아이템이 슬롯에 드롭될 때 호출됩니다. 자식 클래스에서 구현해야 합니다.
    public abstract void OnDrop(PointerEventData eventData);

    // OnPointerEnter: 마우스가 슬롯 위에 올라갈 때 호출됩니다.
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow; // 슬롯 색상을 노란색으로 변경
    }

    // OnPointerExit: 마우스가 슬롯에서 벗어날 때 호출됩니다.
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white; // 슬롯 색상을 원래대로 되돌림
    }

    // SwapItems: 아이템을 교환하는 메서드
    protected virtual void SwapItems(DraggableUI droppedItem)
    {
        if (droppedItem == null) return; // 드롭된 아이템이 없으면 종료

        BaseSlotUI previousSlot = droppedItem.currentSlot;

        // 이전 슬롯에서 아이템 제거
        if (previousSlot != null)
        {
            previousSlot.RemoveItem();
        }

        // 현재 슬롯의 아이템 처리
        if (currentItem != null)
        {
            if (previousSlot != null)
            {
                previousSlot.AddItem(currentItem); // 이전 슬롯에 현재 아이템 추가
            }
            else
            {
                // 이전 슬롯이 없는 경우 (인벤토리에서 새로 드래그된 아이템)
                RemoveItem(); // 현재 아이템 제거
            }
        }

        // 새 아이템 추가
        AddItem(droppedItem);
    }

    // AddItem: 슬롯에 아이템을 추가하는 메서드
    protected virtual void AddItem(DraggableUI item)
    {
        currentItem = item;
        item.currentSlot = this;
        item.transform.SetParent(transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 아이템을 슬롯 중앙에 위치시킴
    }

    // RemoveItem: 슬롯에서 아이템을 제거하는 메서드
    protected virtual void RemoveItem()
    {
        if (currentItem != null)
        {
            currentItem.currentSlot = null;
            currentItem = null;
        }
    }
}