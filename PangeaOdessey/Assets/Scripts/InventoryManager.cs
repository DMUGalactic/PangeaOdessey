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
    private bool isHeadEquipped = false;   // 머리 장비 착용 여부
    private bool isBodyEquipped = false;   // 몸통 장비 착용 여부
    private bool isGlovesEquipped = false; // 장갑 착용 여부
    private bool isShoesEquipped = false;  // 신발 착용 여부

    // 각 장비 아이템의 스탯 증가 값: 장비 착용 시 증가하는 스탯 값 (상수)
    private const int HeadStatValue = 3;   // 머리 장비 스탯 증가 값
    private const int BodyStatValue = 3;   // 몸통 장비 스탯 증가 값
    private const int GlovesStatValue = 3; // 장갑 스탯 증가 값
    private const int ShoesStatValue = 3;  // 신발 스탯 증가 값

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
            case "Head": 
                if (!isHeadEquipped)             // 머리 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    isHeadEquipped = true;       // 머리 장비 장착 상태로 변경
                    baseHealth += HeadStatValue; // 체력 증가
                }
                break;
            case "Body":
                if (!isBodyEquipped)             // 몸통 장비가 장착되어 있지 않은 경우에만 장착
                { 
                    isBodyEquipped = true;       // 몸통 장비 장착 상태로 변경
                    baseHealth += BodyStatValue; // 체력 증가
                }
                break;
            case "Gloves":
                if (!isGlovesEquipped)             // 장갑이 장착되어 있지 않은 경우에만 장착
                { 
                    isGlovesEquipped = true;       // 장갑 장착 상태로 변경
                    baseAttack += GlovesStatValue; // 공격력 증가
                }
                break;
            case "Shoes":
                if (!isShoesEquipped)              // 신발이 장착되어 있지 않은 경우에만 장착
                { 
                    isShoesEquipped = true;        // 신발 장착 상태로 변경
                    baseSpeed += ShoesStatValue;   // 속도 증가
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
            case "Head":
                if (isHeadEquipped)              // 머리 장비가 장착되어 있는 경우에만 해제
                { 
                    isHeadEquipped = false;      // 머리 장비 장착 상태 해제
                    baseHealth -= HeadStatValue; // 증가했던 체력 감소
                }
                break;
            case "Body":
                if (isBodyEquipped)              // 몸통 장비가 장착되어 있는 경우에만 해제
                { 
                    isBodyEquipped = false;      // 몸통 장비 장착 상태 해제
                    baseHealth -= BodyStatValue; // 증가했던 체력 감소
                }
                break;
            case "Gloves":
                if (isGlovesEquipped)              // 장갑이 장착되어 있는 경우에만 해제
                { 
                    isGlovesEquipped = false;      // 장갑 장착 상태 해제
                    baseAttack -= GlovesStatValue; // 증가했던 공격력 감소
                }
                break;
            case "Shoes":
                if (isShoesEquipped)               // 신발이 장착되어 있는 경우에만 해제
                { 
                    isShoesEquipped = false;       // 신발 장착 상태 해제
                    baseSpeed -= ShoesStatValue;   // 증가했던 속도 감소
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