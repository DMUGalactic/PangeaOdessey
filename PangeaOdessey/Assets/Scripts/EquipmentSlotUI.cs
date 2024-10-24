using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentSlotUI : BaseSlotUI
{
    public string slotName; // 슬롯의 이름
    public EquipmentType acceptedEquipmentType; // 이 슬롯에 장착 가능한 장비 타입
    public TMP_Text nameText; // 아이템 이름을 표시할 UI 텍스트
    public TMP_Text healthText; // 총 HP를 표시할 UI 텍스트
    public TMP_Text attackText; // 총 공격력을 표시할 UI 텍스트
    public TMP_Text speedText; // 총 스피드를 표시할 UI 텍스트

    // 드래그된 아이템이 이 슬롯에 드롭될 때 호출되는 메서드
    public override void OnDrop(PointerEventData eventData)
    {
        // 드롭된 아이템의 DraggableUI 컴포넌트를 가져옴
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
    
        // 드롭된 아이템이 유효한지 확인
        if (droppedItem != null && droppedItem.item != null)
        {
            // 드롭된 아이템의 타입이 이 슬롯에서 허용하는 타입인지 확인
            if (droppedItem.item.equipmentType == acceptedEquipmentType)
            {
                SwapItems(droppedItem); // 슬롯에 아이템을 교환
                EquipmentManager.Instance.EquipItem(slotName, droppedItem.item); // 장비 장착
                UpdateStatPanel(droppedItem.item); // 통계 패널 업데이트
                Debug.Log($"장비 장착: {droppedItem.item.itemName}"); // 장비 장착 로그 출력
            }
            else
            {
                Debug.Log("이 슬롯에는 해당 장비를 장착할 수 없습니다."); // 장비 타입 불일치 로그
                droppedItem.ReturnToOriginalSlot(); // 드롭된 아이템 원래 슬롯으로 되돌리기
            }
        }
    }

    // 슬롯에 장착된 아이템의 통계 정보를 업데이트하는 메서드
    public void UpdateStatPanel(Item item)
    {
        var totalStats = EquipmentManager.Instance.GetTotalStats(); // 총 스텟 가져오기
        nameText.text = item != null ? $"{item.itemName}" : "Empty Slot"; // 아이템 이름 설정
        healthText.text = $"Total HP: {totalStats.hp}"; // 총 HP 설정
        attackText.text = $"Total Attack: {totalStats.damage}%"; // 총 공격력 설정
        speedText.text = $"Total Speed: {totalStats.speed}%"; // 총 스피드 설정
    }

    // 슬롯을 비우는 메서드, UI를 초기화
    public void ClearStatPanel()
    {
        UpdateStatPanel(null); // 통계 패널을 비움
    }

    // 아이템을 제거할 때 호출되는 메서드
    protected override void RemoveItem()
    {
        base.RemoveItem(); // 기본 아이템 제거 메서드 호출
        EquipmentManager.Instance.UnequipItem(slotName); // 장비 해제
        UpdateStatPanel(null); // 통계 패널 업데이트
    }
}
