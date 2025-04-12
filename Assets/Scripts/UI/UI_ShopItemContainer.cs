using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_ShopItemContainer : MonoBehaviour
{
    public Button purchaseButton;

    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header(" Lock Elements ")]
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockedSprite, unlockedSprite;
    public bool IsLocked { get; private set; }

    [Header(" Stats ")]
    [SerializeField] private Transform statContainersParent;

    [Header(" Color ")]
    [SerializeField] private Image btnImage;
    [SerializeField] private Image btnOutline;

    [Header(" Purchasing ")]
    private int weaponLevel;
    public WeaponDataSO WeaponData {  get; private set; }
    public ObjectDataSO ObjectData {  get; private set; }

    [Header(" Actions ")]
    public static Action<UI_ShopItemContainer, int> OnPurchased;

    private void Awake()
    {
        CurrencyManager.OnUpdated += CurrencyUpdatedCallback;
    }

    private void OnDestroy()
    {
        CurrencyManager.OnUpdated -= CurrencyUpdatedCallback;
    }
    
    private void CurrencyUpdatedCallback()
    {
        int itemPrice;

        if (WeaponData != null)
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        else
            itemPrice = ObjectData.Price;

        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(itemPrice);
    }

    public void Setup(WeaponDataSO weaponData, int level)
    {
        weaponLevel = level;
        WeaponData = weaponData;

        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (Lv {level + 1})";

        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        priceText.text = weaponPrice.ToString();

        nameText.color = ColorHolder.GetColor(level);
        btnImage.color = ColorHolder.GetColor(level);
        btnOutline.color = ColorHolder.GetOutlineColor(level);

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        SetupStatContainers(calculatedStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(weaponPrice);
    }

    public void Setup(ObjectDataSO objectData)
    {
        ObjectData = objectData;

        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;
        priceText.text = objectData.Price.ToString();

        nameText.color = ColorHolder.GetColor(objectData.Rarity);
        btnImage.color = ColorHolder.GetColor(objectData.Rarity);
        btnOutline.color = ColorHolder.GetOutlineColor(objectData.Rarity);

        SetupStatContainers(objectData.BaseStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(objectData.Price);
    }

    private void SetupStatContainers(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }

    private void Purchase()
    {
        OnPurchased?.Invoke(this, weaponLevel);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }
}
