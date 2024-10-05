using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public TextMeshProUGUI healthText;  // 체력 표시 텍스트
    public TextMeshProUGUI attackText;  // 공격력 표시 텍스트
    public TextMeshProUGUI speedText;   // 속도 표시 텍스트

    // 기본 스탯 값: 플레이어의 기본 스탯을 저장하는 변수들
    private int baseHealth = 0;  // 기본 체력
    private int baseAttack = 0;  // 기본 공격력
    private int baseSpeed = 0;   // 기본 속도

    // 장비 착용 상태: 각 장비 슬롯의 착용 여부를 나타내는 불리언 변수들
    private bool BronzeisHeadEquipped = false;   // 브론즈머리 장비 착용 여부
    private bool SilverisHeadEquipped = false;   // 실버머리 장비 착용 여부
    private bool GoldisHeadEquipped = false;   // 골드머리 장비 착용 여부

    private bool BronzeisBodyEquipped = false;   // 브론즈몸통 장비 착용 여부
    private bool SilverisBodyEquipped = false;   // 실버몸통 장비 착용 여부
    private bool GoldisBodyEquipped = false;   // 골드몸통 장비 착용 여부

    private bool BronzeisGlovesEquipped = false; // 브론즈장갑 착용 여부
    private bool SilverisGlovesEquipped = false; // 실버장갑 착용 여부
    private bool GoldisGlovesEquipped = false; // 골드장갑 착용 여부

    private bool BronzeisShoesEquipped = false;  // 브론즈신발 착용 여부
    private bool SilverisShoesEquipped = false;  // 실버신발 착용 여부
    private bool GoldisShoesEquipped = false;  // 골드신발 착용 여부


    // 각 장비 아이템의 스탯 증가 값: 장비 착용 시 증가하는 스탯 값 (상수)
    private const int BronzeHeadStatValue = 1;   // 브론즈머리 장비 스탯 증가 값
    private const int SilverHeadStatValue = 2;   // 실버머리 장비 스탯 증가 값
    private const int GoldHeadStatValue = 3;   // 골드머리 장비 스탯 증가 값

    private const int BronzeBodyStatValue = 1;   // 브론즈몸통 장비 스탯 증가 값
     private const int SilverBodyStatValue = 2;   // 실버몸통 장비 스탯 증가 값
      private const int GoldBodyStatValue = 3;   // 골드몸통 장비 스탯 증가 값

    private const int BronzeGlovesStatValue = 1; // 브론즈장갑 스탯 증가 값
     private const int SilverGlovesStatValue = 2; // 실버장갑 스탯 증가 값
      private const int GoldGlovesStatValue = 3; // 골드장갑 스탯 증가 값

    private const int BronzeShoesStatValue = 1;  // 브론즈신발 스탯 증가 값
    private const int SilverShoesStatValue = 2;  // 실버신발 스탯 증가 값
    private const int GoldShoesStatValue = 3;  // 골드신발 스탯 증가 값

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 인스턴스 유지
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

    
    /// EquipItem: 아이템을 장착하는 메서드(지정된 아이템을 장착하고 해당 스탯을 증가시킴)
    public void EquipItem(string itemName)
    {
        switch (itemName)
        {
            case "BronzeHead": 
                if (!BronzeisHeadEquipped)             // 머리 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    BronzeisHeadEquipped = true;       // 머리 장비 장착 상태로 변경
                    baseHealth += BronzeHeadStatValue; // 체력 증가
                }
                break;
            case "SilverHead":
                if (!SilverisHeadEquipped)             // 머리 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    SilverisHeadEquipped = true;       // 머리 장비 장착 상태로 변경
                    baseHealth += SilverHeadStatValue; // 체력 증가
                }
                break;
            case "GoldHead":
                if (!GoldisHeadEquipped)             // 머리 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    GoldisHeadEquipped = true;       // 머리 장비 장착 상태로 변경
                    baseHealth += GoldHeadStatValue; // 체력 증가
                }
                break;    
            case "BronzeBody":
                if (!BronzeisBodyEquipped)             // 몸통 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    BronzeisBodyEquipped = true;       // 몸통 장비 장착 상태로 변경
                    baseHealth += BronzeBodyStatValue; // 체력 증가
                }
                break;
            case "SilverBody":
                if (!SilverisBodyEquipped)             // 몸통 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    SilverisBodyEquipped = true;       // 몸통 장비 장착 상태로 변경
                    baseHealth += SilverBodyStatValue; // 체력 증가
                }
                break;
            case "GoldBody":
                if (!GoldisBodyEquipped)             // 몸통 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    GoldisBodyEquipped = true;       // 몸통 장비 장착 상태로 변경
                    baseHealth += GoldBodyStatValue; // 체력 증가
                }
                break;        
            case "BronzeGloves":
                if (!BronzeisGlovesEquipped)             // 장갑이 장착되어 있지 않은 경우에만 장착
                { 
                    BronzeisGlovesEquipped = true;       // 장갑 장착 상태로 변경
                    baseAttack += BronzeGlovesStatValue; // 공격력 증가
                }
                break;
            case "SilverGloves":
                if (!SilverisGlovesEquipped)             // 장갑이 장착되어 있지 않은 경우에만 장착
                { 
                    SilverisGlovesEquipped = true;       // 장갑 장착 상태로 변경
                    baseAttack += SilverGlovesStatValue; // 공격력 증가
                }
                break;
            case "GoldGloves":
                if (!GoldisGlovesEquipped)             // 장갑이 장착되어 있지 않은 경우에만 장착
                { 
                    GoldisGlovesEquipped = true;       // 장갑 장착 상태로 변경
                    baseAttack += GoldGlovesStatValue; // 공격력 증가
                }
                break;
            case "BronzeShoes":
                if (!BronzeisShoesEquipped)              // 신발이 장착되어 있지 않은 경우에만 장착
                { 
                    BronzeisShoesEquipped = true;        // 신발 장착 상태로 변경
                    baseSpeed += BronzeShoesStatValue;   // 속도 증가
                }
                break;
            case "SilverShoes":
                if (!SilverisShoesEquipped)              // 신발이 장착되어 있지 않은 경우에만 장착
                { 
                    SilverisShoesEquipped = true;        // 신발 장착 상태로 변경
                    baseSpeed += SilverShoesStatValue;   // 속도 증가
                }
                break;
            case "GoldShoes":
                if (!GoldisShoesEquipped)              // 신발이 장착되어 있지 않은 경우에만 장착
                { 
                    GoldisShoesEquipped = true;        // 신발 장착 상태로 변경
                    baseSpeed += GoldShoesStatValue;   // 속도 증가
                }
                break;
        }
        UpdateStatsText(); // 스탯 텍스트 업데이트
    }


    /// UnequipItem: 아이템을 해제하는 메서드(지정된 아이템을 해제하고 해당 스탯을 감소시킴)
    public void UnequipItem(string itemName)
    {
        switch (itemName)
        {
            case "BronzeHead":
                if (BronzeisHeadEquipped)              // 머리 장비가 장착되어 있는 경우에만 해제
                { 
                    BronzeisHeadEquipped = false;      // 머리 장비 장착 상태 해제
                    baseHealth -= BronzeHeadStatValue; // 증가했던 체력 감소
                }
                break;
            case "SilverHead":
                if (SilverisHeadEquipped)              // 머리 장비가 장착되어 있는 경우에만 해제
                { 
                    SilverisHeadEquipped = false;      // 머리 장비 장착 상태 해제
                    baseHealth -= SilverHeadStatValue; // 증가했던 체력 감소
                }
                break;
            case "GoldHead":
                if (GoldisHeadEquipped)              // 머리 장비가 장착되어 있는 경우에만 해제
                { 
                    GoldisHeadEquipped = false;      // 머리 장비 장착 상태 해제
                    baseHealth -= GoldHeadStatValue; // 증가했던 체력 감소
                }
                break;
            case "BronzeBody":
                if (BronzeisBodyEquipped)              // 몸통 장비가 장착되어 있는 경우에만 해제
                { 
                    BronzeisBodyEquipped = false;      // 몸통 장비 장착 상태 해제
                    baseHealth -= BronzeBodyStatValue; // 증가했던 체력 감소
                }
                break;
            case "SilverBody":
                if (SilverisBodyEquipped)              // 몸통 장비가 장착되어 있는 경우에만 해제
                { 
                    SilverisBodyEquipped = false;      // 몸통 장비 장착 상태 해제
                    baseHealth -= SilverBodyStatValue; // 증가했던 체력 감소
                }
                break;
            case "GoldBody":
                if (GoldisBodyEquipped)              // 몸통 장비가 장착되어 있는 경우에만 해제
                { 
                    GoldisBodyEquipped = false;      // 몸통 장비 장착 상태 해제
                    baseHealth -= GoldBodyStatValue; // 증가했던 체력 감소
                }
                break;
            case "BronzeGloves":
                if (BronzeisGlovesEquipped)              // 장갑이 장착되어 있는 경우에만 해제
                { 
                    BronzeisGlovesEquipped = false;      // 장갑 장착 상태 해제
                    baseAttack -= BronzeGlovesStatValue; // 증가했던 공격력 감소
                }
                break;
            case "SilverGloves":
                if (SilverisGlovesEquipped)              // 장갑이 장착되어 있는 경우에만 해제
                { 
                    SilverisGlovesEquipped = false;      // 장갑 장착 상태 해제
                    baseAttack -= SilverGlovesStatValue; // 증가했던 공격력 감소
                }
                break;
            case "GoldGloves":
                if (GoldisGlovesEquipped)              // 장갑이 장착되어 있는 경우에만 해제
                { 
                    GoldisGlovesEquipped = false;      // 장갑 장착 상태 해제
                    baseAttack -= GoldGlovesStatValue; // 증가했던 공격력 감소
                }
                break;
            case "BronzeShoes":
                if (BronzeisShoesEquipped)               // 신발이 장착되어 있는 경우에만 해제
                { 
                    BronzeisShoesEquipped = false;       // 신발 장착 상태 해제
                    baseSpeed -= BronzeShoesStatValue;   // 증가했던 속도 감소
                }
                break;
            case "SilverShoes":
                if (SilverisShoesEquipped)               // 신발이 장착되어 있는 경우에만 해제
                { 
                    SilverisShoesEquipped = false;       // 신발 장착 상태 해제
                    baseSpeed -= SilverShoesStatValue;   // 증가했던 속도 감소
                }
                break;
            case "GoldShoes":
                if (GoldisShoesEquipped)               // 신발이 장착되어 있는 경우에만 해제
                { 
                    GoldisShoesEquipped = false;       // 신발 장착 상태 해제
                    baseSpeed -= GoldShoesStatValue;   // 증가했던 속도 감소
                }
                break;
        }
        UpdateStatsText(); // 스탯 텍스트 업데이트
    }

    /// UpdateStatsText: UI에 현재 스탯 값을 표시하는 메서드(플레이어의 현재 스탯을 UI 텍스트에 반영시킴)
    private void UpdateStatsText()
    {
        healthText.text = $"HP {baseHealth}";
        attackText.text = $"Damage {baseAttack}";
        speedText.text = $"Speed {baseSpeed}";
    }
}