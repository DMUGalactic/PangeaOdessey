using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IIventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

    void Awake()
    {
        LoadAllItems();
        FreshSlot();
    }

    private void LoadAllItems()
    {
        string folderPath = "UI/Item"; // 아이템들이 있는 폴더 경로
        items.Clear();

        Item[] loadedItems = Resources.LoadAll<Item>(folderPath);
        items.AddRange(loadedItems);

        if (items.Count == 0)
        {
            Debug.LogWarning($"No items found in {folderPath}. Make sure the path is correct and items are in the Resources folder.");
        }
        else
        {
            Debug.Log($"Loaded {items.Count} items from {folderPath}.");
        }
    }

    public void FreshSlot()
    {
        List<Item> randomItems = new List<Item>(items);
        Shuffle(randomItems);

        int i = 0;
        for (; i < 4 && i < slots.Length && i < randomItems.Count; i++)
        {
            slots[i].item = randomItems[i];
            DraggableUI draggable = slots[i].GetComponentInChildren<DraggableUI>();
            if (draggable != null)
            {
                draggable.SetItem(randomItems[i]);
            }
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }
    // 리스트를 랜덤하게 섞는 메서드
    private void Shuffle(List<Item> list) {
        for (int i = 0; i < list.Count; i++) {
            Item temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void AddItem(Item _item) {
        if (items.Count < slots.Length) {
            items.Add(_item);
            FreshSlot();
        } else {
            print("슬롯이 가득 차 있습니다.");
        }
    }
}