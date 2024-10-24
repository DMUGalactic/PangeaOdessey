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

    foreach (var item in equippedItems.Values) // 장착된 아이템을 순회
    {
        totalHp += item.Hp; // HP 합산

        // 데미지와 스피드는 1을 초과하는 경우에만 퍼센트로 변환하여 더함
        if (item.Damage > 1)
        {
            totalDamage += (item.Damage - 1) * 100; // 퍼센트 변환 후 합산
        }

        if (item.Speed > 1)
        {
            totalSpeed += (item.Speed - 1) * 100; // 퍼센트 변환 후 합산
        }

        // 각 아이템의 정보를 로그로 출력
        Debug.Log($"아이템 정보 - 이름: {item.itemName}, HP: {item.Hp}, " +
                  $"Damage: {(item.Damage > 1 ? (item.Damage - 1) * 100 : 0)}%, " +
                  $"Speed: {(item.Speed > 1 ? (item.Speed - 1) * 100 : 0)}%");
    }

    Debug.Log($"최종 스텟 - HP: {totalHp}, 추가된 Damage: {totalDamage}%, 추가된 Speed: {totalSpeed}%");
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
