using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI; // Image 클래스를 사용하기 위해 추가합니다.
public class EquipmentSlotUI : BaseSlotUI
{
    public int slotID;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text speedText;
    public Image itemImage; // Image 컴포넌트를 위한 변수를 추가합니다.

    private void Start()
    {
        LoadEquippedItem(); // 장착된 아이템 로드
    }

    private void LoadEquippedItem()
    {
        Item item = EquipmentManager.Instance.GetEquippedItem(slotID);
        if (item != null)
        {
            UpdateStatPanel(item);
            SetItemImage(item.itemImage); // 아이템 이미지 설정
        }
        else
        {
            ClearStatPanel();
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DraggableUI droppedItem = eventData.pointerDrag?.GetComponent<DraggableUI>();
    
        if (droppedItem != null && droppedItem.item != null)
        {
            if (droppedItem.item.itemID == slotID)
            {
                SwapItems(droppedItem);
                EquipmentManager.Instance.EquipItem(slotID, droppedItem.item);
                UpdateStatPanel(droppedItem.item);
                SetItemImage(droppedItem.item.itemImage); // 슬롯의 이미지 업데이트
                SaveEquippedItem(droppedItem.item); // 장착된 아이템 저장
                Debug.Log($"장비 장착: 아이템 ID {droppedItem.item.itemID}");
            }
            else
            {
                Debug.Log($"이 슬롯에는 아이템 ID {droppedItem.item.itemID}를 장착할 수 없습니다. 필요한 ID: {slotID}");
                droppedItem.ReturnToOriginalSlot();
            }
        }
    }

    public void UpdateStatPanel(Item item)
    {
        var totalStats = EquipmentManager.Instance.GetTotalStats(); // 총 스탯을 항상 가져오기
        nameText.text = item != null ? $"{item.itemName}" : "Empty Slot";
        healthText.text = $"Total HP: {totalStats.hp}";
        attackText.text = $"Total Attack: {totalStats.damage}%";
        speedText.text = $"Total Speed: {totalStats.speed}%";

        // 아이템 이미지 업데이트 (이미지가 null인 경우 처리)
        if (item != null)
        {
            SetItemImage(item.itemImage);
        }
        else
        {
            SetItemImage(null); // 아이템이 없을 때 이미지 제거
        }
    }

    public void SetItemImage(Sprite newImage)
    {
        if (itemImage != null)
        {
            itemImage.sprite = newImage; // 슬롯에 아이템 이미지 설정
            itemImage.gameObject.SetActive(newImage != null); // 이미지가 있으면 활성화
        }
        // itemImage가 null일 경우 오류를 발생시키지 않음
    }

    public void ClearStatPanel()
    {
        UpdateStatPanel(null);
    }

    protected override void RemoveItem()
    {
        base.RemoveItem();
        EquipmentManager.Instance.UnequipItem(slotID);
        UpdateStatPanel(null);
        SetItemImage(null); // 슬롯에서 아이템 이미지 제거
    }

    private void SaveEquippedItem(Item item)
    {
        // PlayerPrefs에 아이템 ID 저장 (예시)
        PlayerPrefs.SetInt($"EquippedItem_{slotID}", item.itemID);

        // 아이템 이미지를 저장하는 방법 (Sprite는 직접 저장할 수 없으므로 ID로 대체)
        // 추가적으로 이미지 경로 등을 저장할 수 있습니다.
        PlayerPrefs.SetString($"ItemImage_{slotID}", item.itemImage.name); // 이미지 이름으로 저장
        PlayerPrefs.Save(); // PlayerPrefs 저장
    }

    public void LoadSavedItem()
    {
        // 저장된 아이템 로드
        int itemID = PlayerPrefs.GetInt($"EquippedItem_{slotID}", -1);
        if (itemID != -1)
        {
            // 해당 ID의 아이템을 EquipmentManager에서 가져옴
            Item item = EquipmentManager.Instance.GetItemByID(itemID);
            if (item != null)
            {
                UpdateStatPanel(item);
                SetItemImage(item.itemImage); // 아이템 이미지 설정
            }
        }
    }
}