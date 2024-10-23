using System.Collections.Generic;
using UnityEngine;

public class IInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

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


}