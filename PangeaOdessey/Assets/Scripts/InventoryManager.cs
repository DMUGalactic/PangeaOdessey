using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static InventoryManager Instance { get; private set; }

    // UI 텍스트 요소
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
        public ItemType Type;     // 아이템 타입
        public float StatValue;   // 아이템의 스탯 증가 값
        public bool IsEquipped;   // 장착 여부
    }

    // 아이템 정보를 저장하는 딕셔너리 (키: 아이템 이름, 값: 아이템 정보)
    private Dictionary<string, ItemInfo> items = new Dictionary<string, ItemInfo>();

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeItems();  // 아이템 초기화
        }
        else
        {
            Destroy(gameObject);  // 중복 인스턴스 제거
        }
    }

    private void Start()
    {
        UpdateStatsText();  // 초기 스탯 텍스트 업데이트
    }

    // 아이템 초기화 메서드
    private void InitializeItems()
    {
        // 각 아이템 유형별로 아이템 추가
        // 파라미터: 아이템 이름, 아이템 타입, 스탯 증가 값
        
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
        // 딕셔너리에 새 아이템 정보 추가
        items[itemName] = new ItemInfo { Type = type, StatValue = statValue, IsEquipped = false };
    }

    // 아이템 장착 메서드
    public void EquipItem(string itemName)
    {
        // 아이템이 존재하지 않으면 메서드 종료
        if (!items.ContainsKey(itemName)) return;

        var item = items[itemName];
        // 이미 장착 중이면 메서드 종료
        if (item.IsEquipped) return;

        // 아이템 장착 상태로 변경
        item.IsEquipped = true;
        items[itemName] = item;

        // 아이템 타입에 따라 스탯 증가
        switch (item.Type)
        {
            case ItemType.Head:// 머리와 몸통 아이템은 체력(HP)을 증가시킴
            case ItemType.Body:
                baseHealth += (int)item.StatValue;  // StatValue를 정수로 변환하여 baseHealth에 더함
                break;
            case ItemType.Gloves:  // 장갑 아이템은 공격력(Damage)을 퍼센티지로 증가시킴
                baseAttack = (int)(baseAttack * (1 + item.StatValue));  // 현재 공격력에 (1 + StatValue)를 곱하여 증가된 값을 계산 
                break;                                                  // 결과를 정수로 변환하여 baseAttack에 할당
            case ItemType.Shoes:  // 신발 아이템은 속도(Speed)를 퍼센티지로 증가시킴
                baseSpeed = (int)(baseSpeed * (1 + item.StatValue));    // 현재 속도에 (1 + StatValue)를 곱하여 증가된 값을 계산    
                break;                                                  // 결과를 정수로 변환하여 baseSpeed에 할당
        }

        UpdateStatsText();  // UI 텍스트 업데이트
    }

    // 아이템 해제 메서드
    public void UnequipItem(string itemName)
    {
        // 아이템이 존재하지 않으면 메서드 종료
        if (!items.ContainsKey(itemName)) return;

        var item = items[itemName];
        // 장착 중이 아니면 메서드 종료
        if (!item.IsEquipped) return;

        // 아이템 장착 해제 상태로 변경
        item.IsEquipped = false;
        items[itemName] = item;

        // 아이템 타입에 따라 스탯 감소
        switch (item.Type)
        {
            case ItemType.Head: 
            case ItemType.Body:                     // 머리와 몸통 아이템은 체력(HP)을 감소시킴
                baseHealth -= (int)item.StatValue;  // StatValue를 정수로 변환하여 baseHealth에서 뺌
                break;
            case ItemType.Gloves: // 장갑 아이템은 공격력(Damage)을 퍼센티지로 감소시킴
                baseAttack = Mathf.Max(1, (int)(baseAttack / (1 + item.StatValue))); // 현재 공격력을 (1 + StatValue)로 나누어 감소된 값을 계산 // Mathf.Max 함수를 사용하여 결과가 최소 1이 되도록 보장
                break;                                                               // 결과를 정수로 변환하여 baseAttack에 할당
            case ItemType.Shoes: // 신발 아이템은 속도(Speed)를 퍼센티지로 감소시킴
                baseSpeed = Mathf.Max(1, (int)(baseSpeed / (1 + item.StatValue))); // 현재 속도를 (1 + StatValue)로 나누어 감소된 값을 계산 // Mathf.Max 함수를 사용하여 결과가 최소 1이 되도록 보장
                break; // 결과를 정수로 변환하여 baseSpeed에 할당
        } 

        UpdateStatsText();  // UI 텍스트 업데이트
    }

    // 스탯 텍스트 업데이트 메서드
    private void UpdateStatsText()
    {
        // 각 스탯 텍스트 업데이트
        healthText.text = $"HP {baseHealth}";
        attackText.text = $"Damage {baseAttack} (+{CalculatePercentage(ItemType.Gloves)}%)";
        speedText.text = $"Speed {baseSpeed} (+{CalculatePercentage(ItemType.Shoes)}%)";
    }

    // 특정 타입의 아이템에 대한 퍼센티지 계산 메서드
    private int CalculatePercentage(ItemType type)
    {
        // LINQ를 사용하여 장착된 특정 타입 아이템의 StatValue 합계를 계산하고 백분율로 변환
        return (int)(items.Values
            .Where(item => item.Type == type && item.IsEquipped)
            .Sum(item => item.StatValue * 100));
    }
}