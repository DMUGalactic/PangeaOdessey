using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public int bossMode = 0;

    [Header("# Player info")]
    public bool isLive;
    public static int bitCoin = 0;
    public float health = 100f;
    public float maxHealth = 100f;
    public float bossSpawnTime = 20f;
    public int kill;

    [Header(" Game Object")]
    public PoolManager pool;
    public Player player;
    public Text gold;
    public Text timer;

    /* 미적용 
    [Header("# Equipment Reference")]
    public Equipment equipment; // Equipment 스크립트 참조
    */

    [Header("# Boss Info")]
    public GameObject bossPrefab;
    public GameObject bossHUD;
    public float spawnRadius = 5f;
    private bool bossSpawned = false;

    [Header("# Boss Health")]
    public float bossHealth;
    public float maxBossHealth;

    [Header("# Boss Damage")]
    public float bossDamageAmount = 10f;

    [Header("# Panel")]
    public GameObject clear;
    public GameObject gameover;

    public Sprite[] headSpriteList;
    public string[] headNameList;
    public int[] headStatList;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        /* 미적용
        // Equipment의 추가 체력을 maxHealth에 더함
        if (equipment != null)
        {
            maxHealth += equipment.additionalHealth;
        }
        */

        health = maxHealth;
        bitCoin = 0;
        if (bossHUD != null)
        {
            bossHUD.SetActive(false);
        }
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (bossMode == 1 && gameTime >= bossSpawnTime && !bossSpawned)
        {
            SpawnBoss();
        }

        if (gameTime < maxGameTime)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
            string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            timer.text = timeString;
        }

        if (bossMode == 0 && gameTime >= 300 && health > 0)
        {
            if (clear != null)
                clear.SetActive(true);
        }

        gold.text = bitCoin.ToString() + "G";
        if (health <= 0)
        {
            Debug.Log("플레이어 죽음");
            PlayerDead();
        }
    }

    void SpawnBoss()
    {
        Vector2 spawnPosition = (Vector2)player.transform.position + UnityEngine.Random.insideUnitCircle * spawnRadius;
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        BossControls bossControls = boss.GetComponent<BossControls>();
        maxBossHealth = bossControls.health;
        bossHealth = maxBossHealth;

        if (bossHUD != null)
        {
            bossHUD.SetActive(true);
        }

        bossSpawned = true;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;

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

        if (bossHealth <= 0)
        {
            BossDead();
        }
    }

    void BossDead()
    {
        Time.timeScale = 0f;
        clear.SetActive(true);
    }
}
