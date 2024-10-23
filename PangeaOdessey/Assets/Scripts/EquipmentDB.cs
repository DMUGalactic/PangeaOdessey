using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDB", menuName = "Scriptable Object Asset/EquipmentDB")]
public class EquipmentDB : ScriptableObject
{
    public int[] inventory = new int[12]; // ������ ID�� �����ϴ� �κ��丮 �迭
    public List<Item> items; // ������ ����Ʈ

    // ������ ID�� ���� Item�� ��ȯ�ϴ� �޼���
    public Item GetItemById(int id)
    {
        // ID�� �ش��ϴ� �������� ã�� ��ȯ (ID�� ��ȿ���� ������ null ��ȯ)
        return items.Find(item => item.itemID == id);
    }
}
