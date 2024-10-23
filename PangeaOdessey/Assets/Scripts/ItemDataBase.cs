using UnityEngine;

// 아이템 데이터베이스를 생성하기 위한 ScriptableObject 클래스
[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    // 아이템 데이터베이스의 싱글톤 인스턴스
    public static ItemDatabase Instance { get; private set; }

    // 데이터베이스에 포함된 모든 아이템 배열
    public Item[] allItems;

    // 아이템 데이터베이스가 활성화될 때 호출되는 메서드
    private void OnEnable()
    {
        Instance = this; // 현재 인스턴스를 싱글톤으로 설정
    }

    // 주어진 아이템 ID에 해당하는 아이템을 반환하는 메서드
    public Item GetItemByID(int itemID)
    {
        // 모든 아이템을 순회하며 ID가 일치하는 아이템을 찾음
        foreach (var item in allItems)
        {
            if (item.itemID == itemID)
            {
                return item; // 아이템을 찾으면 반환
            }
        }
        return null; // 아이템을 찾지 못했을 때 null 반환
    }

    // 아이템 데이터베이스를 초기화하는 정적 메서드
    public static void Initialize(ItemDatabase database)
    {
        Instance = database; // 데이터베이스를 싱글톤 인스턴스로 설정
    }
}
