using System.Collections.Generic;
using UnityEngine;

// 장비 관리 클래스
public class EquipmentManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EquipmentManager Instance { get; private set; }

    public ItemDatabase itemDatabase; // 인스펙터에서 드래그할 ItemDatabase

    // 장착된 아이템을 저장하는 딕셔너리
    private Dictionary<int, Item> equippedItems = new Dictionary<int, Item>();

    // Awake 메서드에서 인스턴스를 초기화
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 싱글톤 인스턴스 설정
            DontDestroyOnLoad(gameObject); // 게임 오브젝트가 씬 전환 시 파괴되지 않도록 설정
            Debug.Log("EquipmentManager 인스턴스가 초기화되었습니다.");
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재할 경우 현재 오브젝트 파괴
        }
    }

    private void Start()
    {
        itemDatabase.Initialize(); // itemDatabase 초기화
        LoadEquippedItems(); // 장착된 아이템 불러오기
    }

    // 아이템을 장착하는 메서드
    public void EquipItem(int slotID, Item item)
    {
        equippedItems[slotID] = item; // 해당 슬롯에 아이템을 장착
        Debug.Log($"아이템 장착: 슬롯 ID {slotID}, 아이템 ID {item.itemID}");
        UpdateAllEquipmentSlots(); // UI 업데이트
        SaveEquippedItems(); // 장착할 때마다 저장
    }

    // 아이템을 해제하는 메서드
    public void UnequipItem(int slotID)
    {
        if (equippedItems.ContainsKey(slotID)) // 슬롯에 장착된 아이템이 있는 경우
        {
            equippedItems.Remove(slotID); // 아이템 해제
            Debug.Log($"아이템 제거: 슬롯 ID {slotID}");
        }
        UpdateAllEquipmentSlots(); // UI 업데이트
        SaveEquippedItems(); // 해제할 때마다 저장
    }

    // 모든 장착된 아이템의 총 스탯을 계산하는 메서드
    public (int hp, float damage, float speed) GetTotalStats()
    {
        int totalHp = 0;
        float totalDamage = 0f;
        float totalSpeed = 0f;

        foreach (var item in equippedItems.Values) // 장착된 아이템을 순회
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

        // 총합 스탯 정보를 로그로 출력
        Debug.Log($"총합 스탯 - HP: {totalHp}, Damage: {totalDamage}%, Speed: {totalSpeed}%");
        return (totalHp, totalDamage, totalSpeed); // 총합 스탯 반환
    }

    // 모든 장비 슬롯 UI를 업데이트하는 메서드
    private void UpdateAllEquipmentSlots()
    {
        EquipmentSlotUI[] equipmentSlots = FindObjectsOfType<EquipmentSlotUI>(); // 모든 장비 슬롯 UI 찾기
        foreach (var slot in equipmentSlots)
        {
            if (equippedItems.TryGetValue(slot.slotID, out Item item)) // 슬롯에 장착된 아이템이 있는 경우
            {
                slot.UpdateStatPanel(item); // 아이템 정보를 업데이트
                slot.SetItemImage(item.itemImage); // 아이템 이미지 설정
            }
            else
            {
                slot.UpdateStatPanel(null); // 장착된 아이템이 없을 경우 null로 업데이트
                slot.SetItemImage(null); // 이미지 초기화
            }
        }
    }

    // 게임 종료 시 장착된 아이템을 저장하는 메서드
    public void SaveEquippedItems()
    {
        foreach (var kvp in equippedItems) // 장착된 아이템을 순회
        {
            PlayerPrefs.SetInt($"EquippedItem_{kvp.Key}", kvp.Value.itemID); // PlayerPrefs에 저장
        }
        PlayerPrefs.Save(); // PlayerPrefs 저장
        Debug.Log("장착된 아이템이 저장되었습니다.");
    }

    // 게임 시작 시 장착된 아이템을 불러오는 메서드
    public void LoadEquippedItems()
    {
        for (int slotID = 5; slotID <= 8; slotID++) // 5부터 8까지 반복
        {
            if (PlayerPrefs.HasKey($"EquippedItem_{slotID}")) // 해당 슬롯에 저장된 아이템이 있는 경우
            {
                int itemID = PlayerPrefs.GetInt($"EquippedItem_{slotID}"); // 아이템 ID 불러오기
                Item item = FindItemByID(itemID); // 아이템 ID로 아이템을 찾는 메서드
                if (item != null) // 아이템이 존재하는 경우
                {
                    EquipItem(slotID, item); // 아이템 장착
                    Debug.Log($"슬롯 ID {slotID}에 아이템 {item.itemName}이(가) 불러와졌습니다.");
                }
                else
                {
                    Debug.LogWarning($"슬롯 ID {slotID}에 불러올 아이템이 없습니다. 아이템 ID: {itemID}");
                }
            }
            else
            {
                Debug.Log($"슬롯 ID {slotID}에 장착된 아이템이 없습니다.");
            }
        }
        UpdateAllEquipmentSlots(); // 장착된 아이템을 UI에 반영
    }

    // 아이템 ID로 아이템을 찾는 메서드
    private Item FindItemByID(int itemID)
    {
        return itemDatabase.GetItemByID(itemID); // 아이템 데이터베이스에서 아이템 찾기
    }

    // 플레이어 데이터 초기화 메서드
    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteAll(); // 모든 PlayerPrefs 데이터 삭제
        PlayerPrefs.Save(); // 변경 사항 저장
        Debug.Log("모든 플레이어 데이터가 초기화되었습니다.");
    }

    // 장비 초기화 메서드
    public void ResetEquippedItems()
    {
        equippedItems.Clear(); // 장비 목록 비우기
        SaveEquippedItems(); // 비운 목록 저장
        UpdateAllEquipmentSlots(); // UI 업데이트
        Debug.Log("장비가 초기화되었습니다.");
    }

    // 버튼 클릭 시 초기화 메서드
    public void OnResetButtonClicked()
    {
        ResetPlayerData(); // 플레이어 데이터 초기화
        ResetEquippedItems(); // 장비 초기화
    }

    // 특정 슬롯에 장착된 아이템을 반환하는 메서드
    public Item GetEquippedItem(int slotID)
    {
        equippedItems.TryGetValue(slotID, out Item item); // 슬롯 ID로 아이템 검색
        return item; // 아이템 반환
    }

    public Item GetItemByID(int itemID)
    {
        // 아이템을 ID로 검색
        if (itemDatabase.TryGetValue(itemID, out Item item))
        {
            return item;
        }
        return null; // 아이템이 없으면 null 반환
    }
}