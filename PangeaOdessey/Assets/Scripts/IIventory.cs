using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;
    public EquipmentDB equipmentdb;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
            DraggableUI draggable = slots[i].GetComponentInChildren<DraggableUI>();
            if (draggable != null)
            {
                draggable.SetItem(items[i]);
            }
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    private void Update()
    {
        if (equipmentdb != null && equipmentdb.inventory != null)
        {
            // items 리스트를 inventory 배열에 복사
            for (int i = 0; i < equipmentdb.inventory.Length && i < items.Count; i++)
            {
                equipmentdb.inventory[i] = items[i];
            }

            // 남는 슬롯을 null로 채우기
            for (int i = items.Count; i < equipmentdb.inventory.Length; i++)
            {
                equipmentdb.inventory[i] = null;
            }
        }
    }
}
