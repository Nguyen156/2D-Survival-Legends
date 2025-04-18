using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header(" Player Components ")]
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private PlayerObject playerObject;


    [Header(" Elements ")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private Transform pauseInventoryItemsParent;
    [SerializeField] private UI_InventoryItemContainer iventoryItemContainer;
    [SerializeField] private UI_ShopManager shopManagerUI;
    [SerializeField] private UI_InventoryItemInfo itemInfo;

    private void Awake()
    {
        ShopManager.OnItemPurchase += ItemPurchaseCallback;
        WeaponMerger.OnMerge += WeaponMergedCallback;

        GameManager.OnGamePaused += Setup;
    }

    private void OnDestroy()
    {
        ShopManager.OnItemPurchase -= ItemPurchaseCallback;
        WeaponMerger.OnMerge -= WeaponMergedCallback;

        GameManager.OnGamePaused -= Setup;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Setup();
    }

    private void Setup()
    {
        inventoryItemsParent.Clear();
        pauseInventoryItemsParent.Clear();

        Weapon[] weapons = playerWeapon.GetWeapons();

        for(int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] == null)
                continue;

            UI_InventoryItemContainer container = Instantiate(iventoryItemContainer, inventoryItemsParent);
            container.Setup(weapons[i], i, () => ShowItemInfo(container));

            UI_InventoryItemContainer newPauseItem = Instantiate(iventoryItemContainer, pauseInventoryItemsParent);
            newPauseItem.Setup(weapons[i], i, null);
        }


        ObjectDataSO[] objectDatas = playerObject.ObjectList.ToArray();

        for(int i = 0; i < objectDatas.Length; i++)
        {
            UI_InventoryItemContainer newItem = Instantiate(iventoryItemContainer, inventoryItemsParent);
            newItem.Setup(objectDatas[i], () => ShowItemInfo(newItem));

            UI_InventoryItemContainer newPauseItem = Instantiate(iventoryItemContainer, pauseInventoryItemsParent);
            newPauseItem.Setup(objectDatas[i], null);
        }
    }

    private void ShowItemInfo(UI_InventoryItemContainer item)
    {
        if (item.Weapon != null)
            ShowWeaponInfo(item.Weapon, item.Index);
        else if (item.ObjectData != null)
            ShowObjectInfo(item.ObjectData);
    }
    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfo.Setup(weapon);

        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));

        shopManagerUI.ShowItemInfo();
    }

    private void RecycleWeapon(int index)
    {
        playerWeapon.RemoveWeapon(index);

        this.Setup();

        shopManagerUI.HideItemInfo();
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.Setup(objectData);

        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));

        shopManagerUI.ShowItemInfo();
    }

    private void RecycleObject(ObjectDataSO objectToRecycle)
    {
        //Remove object from PlayerObject
        playerObject.RemoveObject(objectToRecycle);

        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        //Destroy the inventory item container
        Setup();

        // Close item info
        shopManagerUI.HideItemInfo();

    }

    private void ItemPurchaseCallback() => Setup();

    private void WeaponMergedCallback(Weapon mergedWeapon)
    {
        Setup();
        itemInfo.Setup(mergedWeapon);
    }
}
