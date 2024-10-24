using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EquipmentManager Instance { get; private set; }

    // 장착된 아이템을 저장하기 위한 딕셔너리
    private Dictionary<string, Item> equippedItems = new Dictionary<string, Item>();

    private void Awake()
    {
        // 싱글톤 패턴을 구현
        if (Instance == null)
        {
            Instance = this; // 현재 인스턴스를 설정
            DontDestroyOnLoad(gameObject); // 씬 변경 시에도 파괴되지 않도록 설정
            Debug.Log("EquipmentManager 인스턴스가 생성되었습니다.");
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있으면 현재 인스턴스를 파괴
        }
    }

    // 아이템을 장착하는 메서드
    public void EquipItem(string slotName, Item item)
    {
        // 슬롯 이름에 따라 아이템을 추가하거나 갱신
        if (equippedItems.ContainsKey(slotName))
        {
            equippedItems[slotName] = item; // 기존 아이템을 새로운 아이템으로 갱신
        }
        else
        {
            equippedItems.Add(slotName, item); // 새로운 슬롯에 아이템 추가
        }
        
        Debug.Log($"아이템 장착: 슬롯 {slotName}, 아이템 {item.itemName}");
        UpdateAllEquipmentSlots(); // 모든 장비 슬롯 업데이트
    }

    // 아이템을 언장착하는 메서드
    public void UnequipItem(string slotName)
    {
        // 슬롯 이름에 따라 아이템을 제거
        if (equippedItems.ContainsKey(slotName))
        {
            equippedItems.Remove(slotName); // 슬롯에서 아이템 제거
            Debug.Log($"아이템 언장착: 슬롯 {slotName}");
        }
        UpdateAllEquipmentSlots(); // 모든 장비 슬롯 업데이트
    }

    // 총 스탯을 계산하여 반환하는 메서드
    public (int hp, float damage, float speed) GetTotalStats()
    {
        int totalHp = 0; // 총 HP 초기화
        float totalDamage = 0f; // 총 데미지 초기화
        float totalSpeed = 0f; // 총 속도 초기화

        // 장착된 아이템을 순회
        foreach (var item in equippedItems.Values)
        {
            totalHp += item.Hp; // HP 합산

            // 데미지와 스피드는 1을 초과하는 경우에만 퍼센트로 변환하여 더함
            if (item.Damage > 1)
            {
                totalDamage += (item.Damage - 1) * 100; // 퍼센트 변환 후 합산
            }

            if (item.Speed > 1)
            {
                totalSpeed += (item.Speed - 1) * 100; // 퍼센트 변환 후 합산
            }

            // 각 아이템의 정보를 로그로 출력
            Debug.Log($"아이템 정보 - 이름: {item.itemName}, HP: {item.Hp}, " +
                      $"Damage: {(item.Damage > 1 ? (item.Damage - 1) * 100 : 0)}%, " +
                      $"Speed: {(item.Speed > 1 ? (item.Speed - 1) * 100 : 0)}%");
        }

        // 최종 스탯 로그 출력
        Debug.Log($"최종 스탯 - HP: {totalHp}, 추가된 Damage: {totalDamage}%, 추가된 Speed: {totalSpeed}%");
        return (totalHp, totalDamage, totalSpeed); // 최종 스탯 반환
    }

    // 모든 장비 슬롯을 업데이트하는 메서드
    private void UpdateAllEquipmentSlots()
    {
        EquipmentSlotUI[] equipmentSlots = FindObjectsOfType<EquipmentSlotUI>(); // 모든 장비 슬롯 UI 찾기
        foreach (var slot in equipmentSlots)
        {
            // 장착된 아이템이 슬롯에 있는 경우 업데이트
            if (equippedItems.TryGetValue(slot.slotName, out Item item))
            {
                slot.UpdateStatPanel(item); // 아이템 정보 업데이트
            }
            else
            {
                slot.UpdateStatPanel(null); // 슬롯에 아이템이 없을 경우 null로 업데이트
            }
        }
    }
}
