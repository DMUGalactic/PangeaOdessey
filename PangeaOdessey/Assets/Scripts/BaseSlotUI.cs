using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseSlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Image image;          
    public DraggableUI currentItem; 

    protected virtual void Awake()
    {
        image = GetComponent<Image>(); 
    }

    public abstract void OnDrop(PointerEventData eventData);

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow; 
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white; 
    }

    protected virtual void SwapItems(DraggableUI droppedItem)
    {
        if (droppedItem == null) return; 

        BaseSlotUI previousSlot = droppedItem.currentSlot;

        if (previousSlot != null)
        {
            previousSlot.RemoveItem();
        }

        if (currentItem != null)
        {
            if (previousSlot != null)
            {
                previousSlot.AddItem(currentItem); 
            }
            else
            {
                RemoveItem(); 
            }
        }

        AddItem(droppedItem);
    }

    protected virtual void AddItem(DraggableUI item)
    {
        currentItem = item;
        item.currentSlot = this;
        item.transform.SetParent(transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
    }

    protected virtual void RemoveItem()
    {
        if (currentItem != null)
        {
            currentItem.currentSlot = null;
            currentItem = null;
        }
    }
}