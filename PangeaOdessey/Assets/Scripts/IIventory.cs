using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // 아이템 리스트

    [SerializeField]
    private Transform slotParent; // 슬롯의 부모 변환
    [SerializeField]
    private Slot[] slots; // 슬롯 배열

    private List<Item> equippedItems = new List<Item>(); // 현재 장착된 아이템 목록

#if UNITY_EDITOR
    private void OnValidate() 
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); // 슬롯 배열 초기화
    }
#endif

    void Awake()
    {
        LoadItemsFromManager(); // 아이템 매니저에서 아이템 로드
        RefreshSlots(); // 슬롯 새로 고침
    }

    // 아이템 매니저에서 아이템과 장착된 아이템 로드
    public void LoadItemsFromManager()
    {
        // 아이템 매니저와 장비 매니저가 초기화되었는지 확인
        if (ItemManager.Instance != null)
        {
            items = ItemManager.Instance.currentItems; // 아이템 매니저에서 아이템 로드
        }
        else
        {
            Debug.LogWarning("ItemManager 인스턴스가 초기화되지 않았습니다.");
            items.Clear(); // 아이템 리스트를 빈 리스트로 초기화
        }

        if (EquipmentManager.Instance != null)
        {
            equippedItems = EquipmentManager.Instance.GetEquippedItems(); // 장착된 아이템 로드
        }
        else
        {
            Debug.LogWarning("EquipmentManager 인스턴스가 초기화되지 않았습니다.");
            equippedItems.Clear(); // 장착된 아이템 리스트를 빈 리스트로 초기화
        }
    }

    // 슬롯 새로 고침
    public void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                // 장착된 장비가 아닌 경우에만 슬롯에 아이템 추가
                if (!equippedItems.Contains(items[i]))
                {
                    slots[i].item = items[i]; // 슬롯에 아이템 설정
                    DraggableUI draggable = slots[i].GetComponentInChildren<DraggableUI>();
                    draggable?.SetItem(items[i]); // 드래그 가능한 UI 설정
                }
                else
                {
                    slots[i].item = null; // 장착된 아이템은 슬롯을 비웁니다.
                }
            }
            else
            {
                slots[i].item = null; // 남은 슬롯 비우기
            }
        }
    }

    // 장착된 장비를 인벤토리로 이동하는 메서드
    public void MoveEquippedItemToInventory(Item item)
    {
        if (equippedItems.Remove(item)) // 장착 해제
        {
            AddItemToInventory(item); // 인벤토리에 아이템 추가
            RefreshSlots(); // 슬롯 업데이트
        }
    }

    // 인벤토리에 아이템 추가
    private void AddItemToInventory(Item item)
    {
        if (item != null && !items.Contains(item)) // 중복 추가 방지
        {
            items.Add(item); // 아이템을 인벤토리에 추가
        }
    }
}
