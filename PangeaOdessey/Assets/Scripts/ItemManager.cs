using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public List<Item> currentItems = new List<Item>(); // 현재 아이템 리스트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 싱글톤 인스턴스 설정
            DontDestroyOnLoad(gameObject); // 게임 오브젝트가 씬 전환 시 파괴되지 않도록 설정
            Debug.Log("ItemManager 인스턴스가 초기화되었습니다.");

            // 아이템을 자동으로 로드
            LoadItems();
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재할 경우 현재 오브젝트 파괴
        }
    }

    // 아이템 로드 메서드
    public void LoadItems()
    {
        currentItems = new List<Item>(ItemDatabase.Instance.GetAllItems()); // 데이터베이스에서 아이템 로드
        Debug.Log("ItemDatabase에서 아이템이 로드되었습니다: " + currentItems.Count + "개의 아이템");
    }

    // 아이템 저장 메서드
    public void SaveItems()
    {
        PlayerPrefs.SetInt("ItemCount", currentItems.Count);
        for (int i = 0; i < currentItems.Count; i++)
        {
            PlayerPrefs.SetInt("Item_" + i, currentItems[i].itemID); // 아이템 ID 저장
        }
        PlayerPrefs.Save(); // 변경 사항 저장
    }

    // 저장된 아이템 로드 메서드
    public void LoadSavedItems()
    {
        currentItems.Clear(); // 현재 아이템 리스트 비우기
        int itemCount = PlayerPrefs.GetInt("ItemCount", 0); // 저장된 아이템 수 가져오기
        for (int i = 0; i < itemCount; i++)
        {
            int itemID = PlayerPrefs.GetInt("Item_" + i, -1); // 저장된 아이템 ID 가져오기
            if (itemID != -1)
            {
                Item item = ItemDatabase.Instance.GetItemByID(itemID); // 아이템 데이터베이스에서 아이템 가져오기
                if (item != null)
                {
                    currentItems.Add(item); // 아이템 추가
                }
            }
        }
        Debug.Log("PlayerPrefs에서 아이템이 로드되었습니다: " + currentItems.Count + "개의 아이템");
    }

    // 아이템 리셋 메서드
    public void ResetItems()
    {
        LoadItems(); // 데이터베이스에서 모든 아이템 다시 로드
        SaveItems(); // 새로운 아이템 상태 저장
    }
}
