using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 슬롯 UI의 기본 클래스. 아이템 드래그 및 슬롯 상호작용을 처리함.
public abstract class BaseSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Image image;          // 슬롯에 표시될 이미지
    public DraggableUI currentItem; // 현재 슬롯에 있는 드래그 가능한 아이템

    // 초기화 메서드
    protected virtual void Awake()
    {
        image = GetComponent<Image>(); // 슬롯의 이미지 컴포넌트를 가져옴
    }

    // 드랍 이벤트 처리 메서드 (구현 필요)
    public abstract void OnDrop(PointerEventData eventData);

    // 포인터가 슬롯에 진입할 때 호출
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow; // 슬롯 색상을 노란색으로 변경
    }

    // 포인터가 슬롯을 벗어날 때 호출
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white; // 슬롯 색상을 기본색으로 변경
    }

    // 아이템 교환 로직을 처리하는 메서드
    protected virtual void SwapItems(DraggableUI droppedItem)
    {
        if (droppedItem == null) return; // 드롭된 아이템이 없으면 종료

        BaseSlotUI previousSlot = droppedItem.currentSlot; // 드롭된 아이템의 이전 슬롯

        // 이전 슬롯에서 아이템 제거
        if (previousSlot != null)
        {
            previousSlot.RemoveItem();
        }

        // 현재 슬롯에 아이템이 있을 경우
        if (currentItem != null)
        {
            if (previousSlot != null)
            {
                previousSlot.AddItem(currentItem); // 이전 슬롯에 현재 아이템 추가
            }
            else
            {
                RemoveItem(); // 아이템 제거
            }
        }

        // 드롭된 아이템을 현재 슬롯에 추가
        AddItem(droppedItem);
    }

    // 아이템을 슬롯에 추가하는 메서드
    protected virtual void AddItem(DraggableUI item)
    {
        currentItem = item; // 현재 아이템 설정
        item.currentSlot = this; // 아이템의 현재 슬롯 설정
        item.transform.SetParent(transform); // 아이템의 부모를 현재 슬롯으로 설정
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 아이템 위치 초기화
    }

    // 슬롯에서 아이템을 제거하는 메서드
    protected virtual void RemoveItem()
    {
        if (currentItem != null)
        {
            currentItem.currentSlot = null; // 아이템의 현재 슬롯을 null로 설정
            currentItem = null; // 현재 아이템을 null로 설정
        }
    }
}
