using UnityEngine;
using System.Collections.Generic;
// 아이템 데이터베이스를 생성하기 위한 ScriptableObject 클래스
[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    // 아이템 데이터베이스의 싱글톤 인스턴스
    public static ItemDatabase Instance { get; private set; }

    // 데이터베이스에 포함된 모든 아이템 배열
    public Item[] allItems;
     // 아이템 딕셔너리
    private Dictionary<int, Item> itemDictionary;

    // 아이템 데이터베이스가 활성화될 때 호출되는 메서드
    private void OnEnable()
    {
        Instance = this; // 현재 인스턴스를 싱글톤으로 설정
        Initialize(); // Initialize 메서드 호출
    }

    // 주어진 아이템 ID에 해당하는 아이템을 반환하는 메서드
    public Item GetItemByID(int itemID)
    {
        foreach (var item in allItems)
        {
            if (item.itemID == itemID)
            {
                return item; // 아이템을 찾으면 반환
            }
        }
        return null; // 아이템을 찾지 못했을 때 null 반환
    }

    
    // Initialize 메서드 추가
    public void Initialize()
    {
        itemDictionary = new Dictionary<int, Item>();

        foreach (var item in allItems)
        {
            itemDictionary[item.itemID] = item; // 아이템을 딕셔너리에 추가
        }

        Debug.Log("ItemDatabase가 초기화되었습니다.");
    }
    public bool TryGetValue(int itemID, out Item item)
    {
        return itemDictionary.TryGetValue(itemID, out item); // 딕셔너리에서 아이템을 시도하여 가져오기
    }
    public Item[] GetAllItems()
    {
        return allItems; // 모든 아이템 배열 반환
    }
}
