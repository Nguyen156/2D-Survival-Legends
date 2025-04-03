using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header(" Elements ")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(health, damage);
        health -= realDamage;

        UpdateUI();

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = (float)health / maxHealth;
        healthText.text = health + " / " + maxHealth;
    }
}
