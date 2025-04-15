using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private Transform shopItemParent;
    [SerializeField] private UI_ShopItemContainer shopItemContainerPrefab;

    [Header(" Player Components")]
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private PlayerObject playerObject;

    [Header(" Reroll ")]
    [SerializeField] private Button rerollButton;
    [SerializeField] private int rerollPrice;
    [SerializeField] private TextMeshProUGUI rerollPriceText;

    [Header(" Actions ")]
    public static Action OnItemPurchase;

    private void Awake()
    {
        UI_ShopItemContainer.OnPurchased += ItemPurchasedCallback;
        CurrencyManager.OnUpdated += CurrencyUpdatedCallback;
    }

    private void OnDestroy()
    {
        UI_ShopItemContainer.OnPurchased -= ItemPurchasedCallback;
        CurrencyManager.OnUpdated -= CurrencyUpdatedCallback;
        
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.SHOP:
                Setup();
                UpdateRerollVisuals();
                break;
        }
    }

    private void Setup()
    {
        List<GameObject> toDestroy = new List<GameObject>();

        for(int i = 0; i < shopItemParent.childCount; i++)
        {
            UI_ShopItemContainer container = shopItemParent.GetChild(i).GetComponent<UI_ShopItemContainer>();

            if (!container.IsLocked)
                toDestroy.Add(container.gameObject);
        }

        while(toDestroy.Count > 0)
        {
            Transform child = toDestroy[0].transform;
            child.SetParent(null);

            Destroy(child.gameObject);
            toDestroy.RemoveAt(0);
        }

        int containersToAdd = 6 - shopItemParent.childCount;
        int weaponContainerCount = Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);
        int objectContainerCount = containersToAdd - weaponContainerCount;

        for(int i = 0; i < weaponContainerCount; i++)
        {
            UI_ShopItemContainer newWeapon = Instantiate(shopItemContainerPrefab, shopItemParent);
            WeaponDataSO randomWeapon = ResourcesManager.GetRandomWeapon();

            newWeapon.Setup(randomWeapon, Random.Range(0, 2));
        }

        for(int i = 0; i < objectContainerCount; i++)
        {
            UI_ShopItemContainer newObject = Instantiate(shopItemContainerPrefab, shopItemParent);

            ObjectDataSO randomObject = ResourcesManager.GetRandomObject();

            newObject.Setup(randomObject);
        }
    }

    public void Reroll()
    {
        Setup();
        CurrencyManager.instance.UseCurrency(rerollPrice);
    }

    public void UpdateRerollVisuals()
    {
        rerollPriceText.text = rerollPrice.ToString();
        rerollButton.interactable = CurrencyManager.instance.HasEnoughCurrency(rerollPrice);
    }

    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisuals();
    }

    private void ItemPurchasedCallback(UI_ShopItemContainer container, int weaponLevel)
    {
        if (container.WeaponData != null)
            TryPurchaseWeapon(container, weaponLevel);
        else
            PurchaseObject(container);
    }

    private void TryPurchaseWeapon(UI_ShopItemContainer container, int weaponLevel)
    {
        if(playerWeapon.TryAddWeapon(container.WeaponData, weaponLevel))
        {
            int price = WeaponStatsCalculator.GetPurchasePrice(container.WeaponData, weaponLevel);
            CurrencyManager.instance.UseCurrency(price);

            Destroy(container.gameObject);

            OnItemPurchase?.Invoke();
        }
    }

    private void PurchaseObject(UI_ShopItemContainer container)
    {
        playerObject.AddObject(container.ObjectData);

        CurrencyManager.instance.UseCurrency(container.ObjectData.Price);

        Destroy(container.gameObject);

        OnItemPurchase?.Invoke();
    }
}
