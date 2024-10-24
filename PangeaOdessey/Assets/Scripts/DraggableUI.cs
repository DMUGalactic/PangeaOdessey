using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 드래그 가능한 UI 요소를 처리하는 클래스
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas; // 부모 캔버스 참조
    private RectTransform rectTransform; // 이 객체의 RectTransform
    private CanvasGroup canvasGroup; // 이 객체의 CanvasGroup, UI의 인터랙션 및 알파 조절
    public BaseSlotUI currentSlot; // 현재 슬롯 참조
    public Item item; // 드래그 중인 아이템

    private void Awake()
    {
        // 부모 캔버스와 RectTransform, CanvasGroup을 초기화
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // 새로운 아이템 설정 메서드
    public void SetItem(Item newItem)
    {
        item = newItem; // 새 아이템 설정
        if (item != null)
        {
            // 아이템 이미지 설정
            GetComponent<Image>().sprite = item.itemImage;
        }
    }

    // 드래그 시작 시 호출되는 메서드
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // 투명도 조정
        canvasGroup.blocksRaycasts = false; // 레이캐스트 차단 해제
        transform.SetParent(canvas.transform); // 캔버스 하위로 이동
        transform.SetAsLastSibling(); // 최상위로 이동
    }

    // 드래그 중에 호출되는 메서드
    public void OnDrag(PointerEventData eventData)
    {
        // 드래그하는 동안 위치 업데이트
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; 
    }

    // 드래그 종료 시 호출되는 메서드
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // 알파 값을 원래대로 되돌림
        canvasGroup.blocksRaycasts = true; // 레이캐스트 차단 복구

        // 현재 슬롯이 없으면 원래 슬롯으로 돌아감
        if (currentSlot == null)
        {
            ReturnToOriginalSlot();
        }
    }

    // 원래 슬롯으로 돌아가는 메서드
    public void ReturnToOriginalSlot()
    {
        if (currentSlot != null)
        {
            // 현재 슬롯의 하위로 이동하고 위치를 (0, 0)으로 설정
            transform.SetParent(currentSlot.transform);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
        }
    }
}
