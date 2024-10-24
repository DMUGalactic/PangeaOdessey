using System.Collections.Generic;
using UnityEngine;

// EquipmentDB 스크립터블 오브젝트 생성 메뉴 등록
[CreateAssetMenu(fileName = "EquipmentDB", menuName = "Scriptable Object Asset/EquipmentDB")]
public class EquipmentDB : ScriptableObject
{
    public int[] inventory = new int[12]; // 인벤토리 ID를 저장하는 배열 (최대 12개의 아이템 ID)
    public List<Item> items; // 아이템 목록

    // 주어진 ID에 해당하는 Item 객체를 반환하는 메서드
    public Item GetItemById(int id)
    {
        // ID에 해당하는 아이템을 목록에서 찾아서 반환 (ID가 유효하지 않으면 null 반환)
        return items.Find(item => item.itemID == id);
    }
}
