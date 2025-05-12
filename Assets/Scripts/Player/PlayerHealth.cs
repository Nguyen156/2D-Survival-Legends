using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header(" Settings ")]
    [SerializeField] private int baseMaxHealth;
    private float maxHealth;
    private float health;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    [Header(" Elements ")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header(" Actions ")]
    public static Action<Vector2> OnAttackDodged;

    //damge, position
    public static Action<int,Vector2> OnDamageTaken;

    private void Awake()
    {
        Enemy.OnDamageTaken += EnemyTakeDamageCallback;
    }

    private void OnDestroy()
    {
        Enemy.OnDamageTaken -= EnemyTakeDamageCallback;
    }

    private void EnemyTakeDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if (health >= maxHealth)
            return;

        float lifeStealValue = damage * lifeSteal;
        float healRecovery = Mathf.Min(lifeStealValue, maxHealth - health);
        
        health += healRecovery;
        UpdateUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth)
            RecoverHealth();
    }

    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;

        if(healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;

            float healthToAdd = Mathf.Min(.1f, maxHealth - health);
            health += healthToAdd;

            UpdateUI();
        }
    }

    public void TakeDamage(int damage)
    {
        if (CanDodge())
        {
            OnAttackDodged?.Invoke(transform.position);
            return;
        }

        float realDamage = Mathf.FloorToInt(damage * Mathf.Clamp(1 - armor / 1000, 1, 10000));
        realDamage = Mathf.Min(health, realDamage);

        health -= realDamage;
        UpdateUI();

        OnDamageTaken?.Invoke((int)realDamage, transform.position);

        if (health <= 0)
            Die();
    }

    private bool CanDodge() => Random.Range(0, 100) < dodge;

    private void Die()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = health / maxHealth;
        healthText.text = (int)health + " / " + maxHealth;
    }

    public void UpdateStats(PlayerStatsManager playerStats)
    {
        float healthToAdd = playerStats.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)healthToAdd;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateUI();

        armor = playerStats.GetStatValue(Stat.Armor);
        lifeSteal = playerStats.GetStatValue(Stat.LifeSteal) / 100;
        dodge = playerStats.GetStatValue(Stat.Dodge);

        healthRecoverySpeed = playerStats.GetStatValue(Stat.HealthRecoverySpeed);
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}
