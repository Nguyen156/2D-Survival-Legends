using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header(" Settings ")]
    private int requiredXp;
    private int currentXp;
    private int level;
    private int levelsEarnedThisWave;

    [Header(" UI ")]
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header(" DEBUG ")]
    [SerializeField] private bool debug;

    private void Awake()
    {
        Candy.OnCollected += CandyCollectedCallback;
    }

    private void OnDestroy()
    {
        Candy.OnCollected -= CandyCollectedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateRequiredXp();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateRequiredXp()
    {
        requiredXp = (level + 1) * 5;
    }

    private void UpdateUI()
    {
        xpBar.fillAmount = (float)currentXp / requiredXp;
        levelText.text = "LV " + (level + 1);
    }

    private void CandyCollectedCallback(Candy candy) 
    {
        currentXp++;

        if (currentXp >= requiredXp)
            LevelUp();

        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        levelsEarnedThisWave++;

        currentXp = 0;

        UpdateRequiredXp();

        PlayerStatsManager.instance.AddPlayerStat(Stat.MaxHealth, 1);

        AudioManager.instance.PlaySFX(4);
    }

    public bool HasLevelUp()
    {
        if(debug)
            return true;

        if(levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }

        return false;
    }
}
