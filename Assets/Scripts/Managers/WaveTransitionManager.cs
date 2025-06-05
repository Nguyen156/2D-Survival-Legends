using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager instance;

    [Header(" Player ")]
    [SerializeField] private PlayerObject playerObject;

    [Header(" Elements ")]
    [SerializeField] private UI_UpgradeButton[] upgradeButtons;
    [SerializeField] private GameObject upgradeButtonParent;

    [Header(" Chest Related ")]
    [SerializeField] private UI_ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;

    [Header(" Settings ")]
    private int chestCollected;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Chest.OnCollected += ChestCollectedCallback;
    }

    private void OnDestroy()
    {
        Chest.OnCollected -= ChestCollectedCallback;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }

    private void TryOpenChest()
    {
        chestContainerParent.Clear();

        if (chestCollected > 0)
            ShowObject();
        else
            ConfigureUpgradeContainer();
    }

    private void ShowObject()
    {
        chestCollected--;

        upgradeButtonParent.SetActive(false);

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObjData = objectDatas[Random.Range(0, objectDatas.Length)];
        
        UI_ChestObjectContainer newChestContainer = Instantiate(chestContainerPrefab, chestContainerParent);
        newChestContainer.Setup(randomObjData);

        newChestContainer.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjData));
        newChestContainer.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(randomObjData));
    }

    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        playerObject.AddObject(objectToTake);
        TryOpenChest();
    }

    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        TryOpenChest();
    }

    private void ConfigureUpgradeContainer()
    {
        upgradeButtonParent.SetActive(true);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);

            Sprite upgradeIcon = ResourcesManager.GetStatIcon((Stat)randomIndex);
            string upgradeName = Enums.FormatStatName((Stat)randomIndex);

            Action action = GetActionToPerform((Stat)randomIndex, out string upgradeValue);

            upgradeButtons[i].Configure(upgradeIcon, upgradeName, upgradeValue);

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
            case Stat.Damage:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.AttackSpeed:
                value = Random.Range(1, 11);
                upgradeValue = "+" + value;
                break;

            case Stat.CriticalChance:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.CriticalPercent:
                value = Random.Range(1, 2);
                upgradeValue = "+" + value;
                break;

            case Stat.MoveSpeed:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.MaxHealth:
                value = Random.Range(1, 10);
                upgradeValue = "+" + value;
                break;

            case Stat.Range:
                value = Random.Range(1, 4);
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

            case Stat.Dodge:
                value = Random.Range(1, 6);
                upgradeValue = "+" + value;
                break;

            case Stat.LifeSteal:
                value = Random.Range(1, 6);
                upgradeValue = $"+{value}";
                break;

            default:
                return () => Debug.Log("Invalid stat");
        }

        //upgradeValue = Enums.FormatStatName(stat) + "\n" + upgradeValue;

        return () => PlayerStatsManager.instance.AddPlayerStat(stat, value);
    }

    private void ChestCollectedCallback()
    {
        chestCollected++;
    }

    public bool HasCollectedChest() => chestCollected > 0;
}
