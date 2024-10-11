using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq; 

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI speedText;

    // 기본 스탯 값
    private int baseHealth = 0;
    private int baseAttack = 1;
    private int baseSpeed = 1;

    // 아이템 타입 열거형
    private enum ItemType { Head, Body, Gloves, Shoes }

    // 아이템 정보를 저장하는 구조체
    private struct ItemInfo
    {
        public ItemType Type;
        public float StatValue;
        public bool IsEquipped;
    }

    // 아이템 정보를 저장하는 딕셔너리
    private Dictionary<string, ItemInfo> items = new Dictionary<string, ItemInfo>();

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateStatsText();
    }

    // 아이템 초기화 메서드
    private void InitializeItems()
    {
        // 헤드 아이템 추가
        AddItem("BronzeHead", ItemType.Head, 1);
        AddItem("SilverHead", ItemType.Head, 2);
        AddItem("GoldHead", ItemType.Head, 3);

        // 바디 아이템 추가
        AddItem("BronzeBody", ItemType.Body, 2);
        AddItem("SilverBody", ItemType.Body, 4);
        AddItem("GoldBody", ItemType.Body, 6);

        // 글러브 아이템 추가
        AddItem("BronzeGloves", ItemType.Gloves, 0.1f);
        AddItem("SilverGloves", ItemType.Gloves, 0.2f);
        AddItem("GoldGloves", ItemType.Gloves, 0.3f);

        // 신발 아이템 추가
        AddItem("BronzeShoes", ItemType.Shoes, 0.1f);
        AddItem("SilverShoes", ItemType.Shoes, 0.2f);
        AddItem("GoldShoes", ItemType.Shoes, 0.3f);
    }

    // 아이템 추가 메서드
    private void AddItem(string itemName, ItemType type, float statValue)
    {
        items[itemName] = new ItemInfo { Type = type, StatValue = statValue, IsEquipped = false };
    }

    // 아이템 장착 메서드
    public void EquipItem(string itemName)
    {
        if (!items.ContainsKey(itemName)) return;

        var item = items[itemName];
        if (item.IsEquipped) return;

        item.IsEquipped = true;
        items[itemName] = item;

        switch (item.Type)
        {
            case ItemType.Head:
            case ItemType.Body:
                baseHealth += (int)item.StatValue;
                break;
            case ItemType.Gloves:
                baseAttack = (int)(baseAttack * (1 + item.StatValue));
                break;
            case ItemType.Shoes:
                baseSpeed = (int)(baseSpeed * (1 + item.StatValue));
                break;
        }

        UpdateStatsText();
    }

    // 아이템 해제 메서드
    public void UnequipItem(string itemName)
    {
        if (!items.ContainsKey(itemName)) return;

        var item = items[itemName];
        if (!item.IsEquipped) return;

        item.IsEquipped = false;
        items[itemName] = item;

        switch (item.Type)
        {
            case ItemType.Head:
            case ItemType.Body:
                baseHealth -= (int)item.StatValue;
                break;
            case ItemType.Gloves:
                baseAttack = Mathf.Max(1, (int)(baseAttack / (1 + item.StatValue)));
                break;
            case ItemType.Shoes:
                baseSpeed = Mathf.Max(1, (int)(baseSpeed / (1 + item.StatValue)));
                break;
        }

        UpdateStatsText();
    }

    // 스탯 텍스트 업데이트 메서드
    private void UpdateStatsText()
    {
        healthText.text = $"HP {baseHealth}";
        attackText.text = $"Damage {baseAttack} (+{CalculatePercentage(ItemType.Gloves)}%)";
        speedText.text = $"Speed {baseSpeed} (+{CalculatePercentage(ItemType.Shoes)}%)";
    }

    // 특정 타입의 아이템에 대한 퍼센티지 계산 메서드
    private int CalculatePercentage(ItemType type)
    {
        return (int)(items.Values
            .Where(item => item.Type == type && item.IsEquipped)
            .Sum(item => item.StatValue * 100));
    }
}