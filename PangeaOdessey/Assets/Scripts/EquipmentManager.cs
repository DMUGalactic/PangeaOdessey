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
        UpdateAllEquipmentSlots();
    }

    public void UnequipItem(string slotName)
    {
        if (equippedItems.ContainsKey(slotName))
        {
            equippedItems.Remove(slotName);
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
        }

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