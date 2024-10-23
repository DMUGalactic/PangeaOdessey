using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    private Dictionary<string, Item> equippedItems = new Dictionary<string, Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("EquipmentManager 인스턴스가 초기화되었습니다.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipItem(string slotName, Item item)
    {
        // Replace or add the item in the specified slot
        equippedItems[slotName] = item;
        Debug.Log($"아이템 장착: 슬롯 {slotName}, 아이템 {item.name}");
        UpdateAllEquipmentSlots();
    }

    public void UnequipItem(string slotName)
    {
        if (equippedItems.ContainsKey(slotName))
        {
            equippedItems.Remove(slotName);
            Debug.Log($"아이템 제거: 슬롯 {slotName}");
        }
        UpdateAllEquipmentSlots();
    }

    public (int hp, float damage, float speed) GetTotalStats()
{
    int totalHp = 0;
    float totalDamage = 0f;
    float totalSpeed = 0f;

    foreach (var item in equippedItems.Values)
    {
        totalHp += item.Hp;

        // Adjust calculation to ensure no negative percentages
        if (item.Damage > 1)
        {
            totalDamage += (item.Damage - 1) * 100;
        }

        if (item.Speed > 1)
        {
            totalSpeed += (item.Speed - 1) * 100;
        }

        Debug.Log($"아이템 정보 - 이름: {item.name}, HP: {item.Hp}, Damage: {(item.Damage > 1 ? (item.Damage - 1) * 100 : 0)}%, Speed: {(item.Speed > 1 ? (item.Speed - 1) * 100 : 0)}%");
    }

    Debug.Log($"총합 스탯 - HP: {totalHp}, Damage: {totalDamage}%, Speed: {totalSpeed}%");
    return (totalHp, totalDamage, totalSpeed);
}
    private void UpdateAllEquipmentSlots()
    {
        EquipmentSlotUI[] equipmentSlots = FindObjectsOfType<EquipmentSlotUI>();
        foreach (var slot in equipmentSlots)
        {
            if (equippedItems.TryGetValue(slot.slotName, out Item item))
            {
                slot.UpdateStatPanel(item);
            }
            else
            {
                slot.UpdateStatPanel(null);
            }
        }
    }
}
