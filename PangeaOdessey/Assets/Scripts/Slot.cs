using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : BaseSlotUI
{
    [SerializeField] private new Image image;
    private Item _item;
    public EquipmentDB equipmentdb;  // EquipmentDB�� ���� �����͸� ������
    public int slotIndex; // �� ������ �ε��� (EquipmentDB������ ���� ��ġ)

    public Item item
    {
        get
        {
            return _item;
        }
        set
        {
            // EquipmentDB���� ������ ���� �ݿ�
            if (equipmentdb != null)
            {
                int itemId = equipmentdb.inventory[slotIndex]; // Inventory �迭���� ������ ID ��������
                _item = equipmentdb.GetItemById(itemId); // EquipmentDB���� ���� ������ ��������

                if (_item != null)
                {
                    image.sprite = _item.itemImage; // ������ �̹��� ����
                    image.color = new Color(1, 1, 1, 1); // �������� ������ �̹��� ǥ��
                }
                else
                {
                    image.color = new Color(1, 1, 1, 0); // �������� ������ �̹��� ����
                }
            }
        }
    }

    public void SetDraggableItem(DraggableUI draggable)
    {
        if (draggable != null && item != null)
        {
            draggable.SetItem(item); // �巡�� ������ UI�� ������ ����
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
        if (droppedItem != null)
        {
            SwapItems(droppedItem); // ������ ��ü ����
        }
    }
}
