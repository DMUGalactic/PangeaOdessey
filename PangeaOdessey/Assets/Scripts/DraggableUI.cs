using UnityEngine;
using UnityEngine.EventSystems;

// DraggableUI 클래스: 드래그 가능한 UI 요소를 관리합니다.
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;              // UI가 속한 캔버스
    private RectTransform rectTransform; // 이 UI 요소의 RectTransform
    private CanvasGroup canvasGroup;     // 투명도와 레이캐스트 블로킹을 위한 CanvasGroup
    public BaseSlotUI currentSlot;       // 현재 이 아이템이 위치한 슬롯
    public string itemName;              // 아이템의 이름

    private void Awake()
    {
        // 필요한 컴포넌트들을 초기화합니다.
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // 드래그 시작 시 호출됩니다.
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;                 // 드래그 중인 아이템을 반투명하게 만듭니다.
        canvasGroup.blocksRaycasts = false;       // 레이캐스트를 막아 아이템 아래의 UI와 상호작용할 수 있게 합니다.
        transform.SetParent(canvas.transform);    // 아이템을 캔버스의 직접적인 자식으로 만들어 다른 UI 위에 표시되게 합니다.
        transform.SetAsLastSibling();             // 아이템을 가장 위에 표시되도록 합니다.
    }

    // 드래그 중 지속적으로 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 커서를 따라 아이템을 이동시킵니다.
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // 드래그가 끝났을 때 호출됩니다.
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;               // 아이템의 투명도를 원래대로 되돌립니다.
        canvasGroup.blocksRaycasts = true;    // 레이캐스트 블로킹을 다시 활성화합니다.
        if (currentSlot == null)
        {
            // 아이템이 유효한 슬롯에 놓이지 않았을 경우, 원래 위치로 돌아갑니다.
            ReturnToOriginalSlot();
        }
    }

    // 아이템을 원래 슬롯으로 되돌리는 메서드입니다.
    public void ReturnToOriginalSlot()
    {
        if (currentSlot != null)
        {
            transform.SetParent(currentSlot.transform);  // 원래 슬롯의 자식으로 되돌립니다.
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  // 슬롯 중앙에 위치시킵니다.
        }
    }
}