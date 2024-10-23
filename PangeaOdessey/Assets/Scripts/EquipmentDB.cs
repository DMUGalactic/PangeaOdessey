using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDB", menuName = "Scriptable Object Asset/EquipmentDB")]
public class EquipmentDB : ScriptableObject
{
    public int[] inventory = new int[12]; // 아이템 ID를 저장하는 인벤토리 배열
    public List<Item> items; // 아이템 리스트

    // 아이템 ID를 통해 Item을 반환하는 메서드
    public Item GetItemById(int id)
    {
        // ID에 해당하는 아이템을 찾아 반환 (ID가 유효하지 않으면 null 반환)
        return items.Find(item => item.itemID == id);
    }
}
