using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public int bossMode = 0; // 0이면 일반맵, 1이면 보스맵
    [Header("# Player info")]
    public bool isLive;
    public static int bitCoin = 0;
    public float health = 100f; // int -> float
    public float maxHealth = 100f; // int -> float
    public float bossSpawnTime = 20f; // 보스 스폰 시간
    public int kill;

    [Header(" Game Object")]
    public PoolManager pool;
    public Player player;
    public Text gold;
    public Text timer;
    
    [Header("# Boss Info")]
    public GameObject bossPrefab; // 보스 프리팹
    public GameObject bossHUD; // 보스 HP UI
    public float spawnRadius = 5f; // 플레이어 주위 스폰 반경
    private bool bossSpawned = false; // 보스가 한 번만 스폰되도록 설정

    [Header("# Boss Health")]
    public float bossHealth;
    public float maxBossHealth;

    [Header("# Boss Damage")]
    public float bossDamageAmount = 20f;
    

    [Header("# Panel")]
    public GameObject clear;
    public GameObject gameover;

    public StageData stageData;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
        bitCoin = 0;
        if (bossHUD != null)
        {
            bossHUD.SetActive(false); // 게임 시작 시 보스 HP UI 비활성화
        }
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        
        if (bossMode == 1 && gameTime >= bossSpawnTime && !bossSpawned)
        {
            SpawnBoss();
            Debug.Log("보스 스폰");
        }
        
        if (gameTime < maxGameTime)
        {
            // 게임 오버 로직을 여기에 추가합니다.
            TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
            string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            timer.text = timeString;
        }
        if(bossMode == 0 && gameTime>=300 && health > 0){
            // 일반맵 게임 클리어 시
            // 클리어 패널 활성화 게임 일시정지
            if(clear != null)
                clear.SetActive(true);
        }

        gold.text = bitCoin.ToString()+ "G";
        if(health < 0){
            Debug.Log("플레이어 죽음");
            PlayerDead();
        }
    }
    
    void SpawnBoss()
    {
        Vector2 spawnPosition = (Vector2)player.transform.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        
        // 보스 체력 설정
        bossHealth = maxBossHealth;

        if (bossHUD != null)
        {
            Debug.Log("HUD 활성화");
            bossHUD.SetActive(true); // 보스가 스폰될 때 HP UI 활성화
        }

        bossSpawned = true; // 보스를 한 번만 스폰되도록 설정
    }
    

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;

        // 필요 시 플레이어가 죽었을 때 로직 추가
        if (health == 0)
        {
            PlayerDead();
        }
    }

    void PlayerDead()
    {
        gameover.SetActive(true);
    }
    
    public void TakeBossDamage(float amount)
    {
        bossHealth -= amount;
        if (bossHealth < 0) bossHealth = 0;

        // 필요 시 보스가 죽었을 때 로직 추가
        if (bossHealth <= 0)
        {
            Invoke("BossDead", 1.03f);
        }
    }

    void BossDead()
    {
        Time.timeScale = 0f;
        clear.SetActive(true);
    }

    public void StageClear()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if(stageData != null)
        {
            switch(currentScene)
            {
                case "Grass":
                    stageData.grassStage_2 = true;
                    break;
                case "GrassBoss":
                    stageData.iceStage_1 = true;
                    break;
                case "Ice":
                    stageData.iceStage_2 = true;
                    break;
                case "IceBoss":
                    stageData.fireStage_1 = true;
                    break;
                case "Fire":
                    stageData.fireStage_2 = true;
                    break;
                case "FireBoss":
                    stageData.darkStage_1 = true;
                    break;
                case "Dark":
                    stageData.darkStage_2 = true;
                    break;

                default:
                    break;

            }
            
        }
            
    }

}

