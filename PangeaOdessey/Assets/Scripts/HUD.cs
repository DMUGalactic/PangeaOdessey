using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum Infotype { Exp, Level, Kill, Time, Health, BossHP }
    public Infotype type;

    Text myText;
    public Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case Infotype.Exp:
                // Implement experience related UI updates
                break;

            case Infotype.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case Infotype.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case Infotype.Health:
                UpdateHealthUI();
                break;

            case Infotype.BossHP:
                UpdateBossHPUI();
                break;
        }
    }

    void UpdateHealthUI()
    {
        float curHealth = GameManager.instance.health;
        float maxHealth = GameManager.instance.GetMaxHealth();

        // 현재 체력이 최대 체력을 초과하지 않도록 조정
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
            GameManager.instance.health = maxHealth;
        }

        mySlider.value = curHealth / maxHealth;
    }

    void UpdateBossHPUI()
    {
        float curBossHealth = GameManager.instance.bossHealth;
        float maxBossHealth = GameManager.instance.maxBossHealth;
        mySlider.value = curBossHealth / maxBossHealth;
    }
}