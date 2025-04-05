using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private UI_UpgradeButton[] upgradeButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainer();
                break;
        }
    }

    private void ConfigureUpgradeContainer()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);

            string upgradeName = Enums.FormatStatName((Stat)randomIndex);

            Action action = GetActionToPerform((Stat)randomIndex, out string upgradeValue);

            upgradeButtons[i].Configure(null, upgradeName, upgradeValue);

            upgradeButtons[i].Button.onClick.RemoveAllListeners();
            upgradeButtons[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeButtons[i].Button.onClick.AddListener(() => BonusSelectedCallback());
        }
    }

    private void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();
    }

    private Action GetActionToPerform(Stat stat, out string upgradeValue)
    {
        upgradeValue = "";
        float value = 0;

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value + '%';
                break;

            case Stat.AttackSpeed:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value + '%';
                break;

            case Stat.CriticalChance:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value + '%';
                break;

            case Stat.CriticalPercent:
                value = Random.Range(1, 6f);
                upgradeValue = "+" + value.ToString("F2") + 'x';
                break;

            case Stat.MoveSpeed:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value + '%';
                break;

            case Stat.MaxHealth:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value;
                break;

            case Stat.Range:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.Armor:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.Luck:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value + '%';
                break;

            case Stat.Dodge:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value + "%";
                break;

            case Stat.LifeSteal:
                value = Random.Range(1, 11);
                upgradeValue = $"+{value}%";
                break;

            default:
                return () => Debug.Log("Invalid stat");
        }

        return () => PlayerStatsManager.instance.AddPlayerStat(stat, value);
    }
}
