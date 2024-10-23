using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : BaseSlotUI
{
    [SerializeField] private new Image image;
    private Item _item;

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SetDraggableItem(DraggableUI draggable)
    {
        if (draggable != null && item != null)
        {
            draggable.SetItem(item);
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
        if (droppedItem != null)
        {
            SwapItems(droppedItem);
        }
    }
}