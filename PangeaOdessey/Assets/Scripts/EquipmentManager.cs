using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    // 싱글톤 인스턴스

    public static EquipmentManager Instance { get; private set; }

    private Dictionary<string, Item> equippedItems = new Dictionary<string, Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("EquipmentManager �ν��Ͻ��� �ʱ�ȭ�Ǿ����ϴ�.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipItem(string slotName, Item item)
    {
        if (equippedItems.ContainsKey(slotName))
        {
            equippedItems[slotName] = item;
        }
        else
        {
            equippedItems.Add(slotName, item);
        }
        Debug.Log($"������ ����: ���� {slotName}, ������ {item.name}");
        UpdateAllEquipmentSlots();
    }

    public void UnequipItem(string slotName)
    {
        if (equippedItems.ContainsKey(slotName))
        {
            equippedItems.Remove(slotName);
            Debug.Log($"������ ����: ���� {slotName}");
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
            totalDamage += item.Damage;
            totalSpeed += item.Speed;
            Debug.Log($"������ ���� - �̸�: {item.name}, HP: {item.Hp}, Damage: {item.Damage}, Speed: {item.Speed}");
        }

        Debug.Log($"���� ���� - HP: {totalHp}, Damage: {totalDamage}, Speed: {totalSpeed}");
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
