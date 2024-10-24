using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // 아이템 리스트
    
    [SerializeField]
    private Transform slotParent; // 슬롯의 부모 변환
    [SerializeField]
    private Slot[] slots; // 슬롯 배열

#if UNITY_EDITOR
    private void OnValidate() 
    {
        slots = slotParent.GetComponentsInChildren<Slot>(); // 슬롯 배열 초기화
    }
#endif

    void Awake()
    {
        LoadItemsFromManager(); // 아이템 매니저에서 아이템 로드
        FreshSlot(); // 슬롯 새로 고침
    }

    public void LoadItemsFromManager()
    {
        items = ItemManager.Instance.currentItems; // 아이템 매니저에서 아이템 로드
    }

    public void FreshSlot()
    {
        // 슬롯에 아이템 배치
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].item = items[i]; // 슬롯에 아이템 설정
                DraggableUI draggable = slots[i].GetComponentInChildren<DraggableUI>();
                if (draggable != null)
                {
                    draggable.SetItem(items[i]); // 드래그 가능한 UI 설정
                }
            }
            else
            {
                slots[i].item = null; // 남은 슬롯 비우기
            }
        }
    }
}