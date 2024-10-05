using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
 
    // UI 텍스트 요소들
    public TextMeshProUGUI healthText;  // 체력 표시 텍스트
    public TextMeshProUGUI attackText;  // 공격력 표시 텍스트
    public TextMeshProUGUI speedText;   // 속도 표시 텍스트

    private int baseHealth = 0;  // 기본 체력
    private int baseAttack = 0; // 기본 공격력
    private int baseSpeed = 0;  // 기본 속도

    // 장비 타입
    public enum EquipmentType
    {
        Head,
        Body,
        Gloves,
        Shoes
    }


    // 장비 등급
    public enum EquipmentTier
    {
        Bronze,
        Silver,
        Gold
    }

     // 장비 정보를 담는 클래스
    public class Equipment
    {
        public string Name;        // 장비 이름
        public EquipmentType Type; // 장비 타입
        public EquipmentTier Tier; // 장비 등급
        public int StatBonus;      // 장비 착용 시 증가하는 스탯 값
        public bool IsEquipped;    // 장비 착용 여부
    }

    // 모든 장비 아이템을 저장하는 딕셔너리
    private Dictionary<string, Equipment> equipments = new Dictionary<string, Equipment>
    {
        {"BronzeHead", new Equipment {Name = "BronzeHead", Type = EquipmentType.Head, Tier = EquipmentTier.Bronze, StatBonus = 1, IsEquipped = false}},
        {"SilverHead", new Equipment {Name = "SilverHead", Type = EquipmentType.Head, Tier = EquipmentTier.Silver, StatBonus = 2, IsEquipped = false}},
        {"GoldHead", new Equipment {Name = "GoldHead", Type = EquipmentType.Head, Tier = EquipmentTier.Gold, StatBonus = 3, IsEquipped = false}},
        {"BronzeBody", new Equipment {Name = "BronzeBody", Type = EquipmentType.Body, Tier = EquipmentTier.Bronze, StatBonus = 1, IsEquipped = false}},
        {"SilverBody", new Equipment {Name = "SilverBody", Type = EquipmentType.Body, Tier = EquipmentTier.Silver, StatBonus = 2, IsEquipped = false}},
        {"GoldBody", new Equipment {Name = "GoldBody", Type = EquipmentType.Body, Tier = EquipmentTier.Gold, StatBonus = 3, IsEquipped = false}},
        {"BronzeGloves", new Equipment {Name = "BronzeGloves", Type = EquipmentType.Gloves, Tier = EquipmentTier.Bronze, StatBonus = 1, IsEquipped = false}},
        {"SilverGloves", new Equipment {Name = "SilverGloves", Type = EquipmentType.Gloves, Tier = EquipmentTier.Silver, StatBonus = 2, IsEquipped = false}},
        {"GoldGloves", new Equipment {Name = "GoldGloves", Type = EquipmentType.Gloves, Tier = EquipmentTier.Gold, StatBonus = 3, IsEquipped = false}},
        {"BronzeShoes", new Equipment {Name = "BronzeShoes", Type = EquipmentType.Shoes, Tier = EquipmentTier.Bronze, StatBonus = 1, IsEquipped = false}},
        {"SilverShoes", new Equipment {Name = "SilverShoes", Type = EquipmentType.Shoes, Tier = EquipmentTier.Silver, StatBonus = 2, IsEquipped = false}},
        {"GoldShoes", new Equipment {Name = "GoldShoes", Type = EquipmentType.Shoes, Tier = EquipmentTier.Gold, StatBonus = 3, IsEquipped = false}}
    }; // 각 장비 아이템 정의

    // Awake: 스크립트가 로드될 때 호출되는 메서드
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 오브젝트 유지 
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    private void Start()
    {
        UpdateStatsText(); // 초기 스탯 텍스트 업데이트
    }


    // EquipItem: 아이템을 장착하는 메서드
     public void EquipItem(string itemName)
    {
        if (equipments.TryGetValue(itemName, out Equipment equipment))
        {
            equipment.IsEquipped = true; // 장비 해제 상태로 변경
            UpdateStats(); // 스탯 업데이트
        }
    }

    // UnequipItem: 아이템을 해제하는 메서드
    public void UnequipItem(string itemName)
    {
        if (equipments.TryGetValue(itemName, out Equipment equipment))
        {
            equipment.IsEquipped = false; // 장비 해제 상태로 변경
            UpdateStats(); // 스탯 업데이트
        } 
    }

    // ToggleEquipment: 장비 착용/해제를 토글하는 메서드
    public void ToggleEquipment(string itemName)
    {
        if (equipments.TryGetValue(itemName, out Equipment equipment))
        {
            equipment.IsEquipped = !equipment.IsEquipped; // 착용 상태 반전
            UpdateStats(); // 스탯 업데이트
        }
    }

    // UpdateStats: 모든 장비의 효과를 적용하여 스탯을 업데이트하는 메서드
    private void UpdateStats()
    {
        baseHealth = baseAttack = baseSpeed = 0;           // 스탯 초기화
        foreach (var equipment in equipments.Values)
        {
            if (equipment.IsEquipped)
            {
                switch (equipment.Type)
                {
                    case EquipmentType.Head: 
                    case EquipmentType.Body:
                        baseHealth += equipment.StatBonus; // 체력 증가
                        break;
                    case EquipmentType.Gloves:
                        baseAttack += equipment.StatBonus; // 공격력 증가
                        break;
                    case EquipmentType.Shoes:
                        baseSpeed += equipment.StatBonus; // 속도 증가
                        break;
                }
            }
        }
        UpdateStatsText(); // UI 텍스트 업데이트
    }

    // UpdateStatsText: UI에 현재 스탯 값을 표시하는 메서드
    private void UpdateStatsText()
    {
        healthText.text = $"HP {baseHealth}";
        attackText.text = $"Damage {baseAttack}";
        speedText.text = $"Speed {baseSpeed}";
    }
}
