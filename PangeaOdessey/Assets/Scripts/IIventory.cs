using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // 전체 아이템 리스트
    public List<Item> selectedItems = new List<Item>(); // 선택된 아이템 리스트 추가

    [SerializeField]
    private Transform slotParent; // 슬롯 부모 오브젝트
    [SerializeField]
    private Slot[] slots; // 슬롯 배열

#if UNITY_EDITOR
    // 에디터에서 슬롯 컴포넌트를 자동으로 가져오기 위한 메서드
    private void OnValidate() 
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); // 슬롯 부모에서 모든 슬롯 컴포넌트 가져오기
    }
#endif

    void Awake()
    {
        FreshSlot(); // 인벤토리 초기화
    }

    // 슬롯을 새로 고치는 메서드
    public void FreshSlot()
    {
        selectedItems.Clear(); // 선택된 아이템 리스트 초기화
        SelectRandomItems(); // 랜덤 아이템 선택

        int i = 0;
        // 선택된 아이템을 슬롯에 할당
        for (; i < selectedItems.Count && i < slots.Length; i++)
        {
            slots[i].item = selectedItems[i]; // 슬롯에 아이템 설정
            DraggableUI draggable = slots[i].GetComponentInChildren<DraggableUI>();
            if (draggable != null)
            {
                draggable.SetItem(selectedItems[i]); // 드래그 가능 UI에 아이템 설정
            }
        }
        // 남은 슬롯은 null로 설정
        for (; i < slots.Length; i++)
        {
            slots[i].item = null; // 슬롯에 아이템이 없을 경우 null 설정
        }
    }

    // 랜덤 아이템을 선택하는 메서드
    private void SelectRandomItems()
    {
        HashSet<EquipmentType> selectedTypes = new HashSet<EquipmentType>(); // 선택된 장비 타입을 저장할 해시셋
        
        // 4개의 아이템을 선택할 때까지 반복
        while (selectedItems.Count < 4)
        {
            int randomIndex = Random.Range(0, items.Count); // 랜덤 인덱스 생성
            Item randomItem = items[randomIndex]; // 랜덤 아이템 선택

            // 선택된 타입에 없는 아이템만 추가
            if (!selectedTypes.Contains(randomItem.equipmentType) && selectedItems.Count < 4)
            {
                selectedItems.Add(randomItem); // 선택된 아이템 리스트에 추가
                selectedTypes.Add(randomItem.equipmentType); // 장비 타입을 해시셋에 추가
            }
        }
    }
}
