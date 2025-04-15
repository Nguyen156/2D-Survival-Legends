using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItemContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image container;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public int Index {  get; private set; }
    public Weapon Weapon {  get; private set; }
    public ObjectDataSO ObjectData {  get; private set; }


    public void Setup(Color containerColor, Sprite itemIcon)
    {
        container.color = containerColor;
        icon.sprite = itemIcon;
    }

    public void Setup(Weapon weapon, int index, Action clickedCallback)
    {
        Weapon = weapon;
        Index = index;

        Color color = ColorHolder.GetColor(weapon.Level);
        Sprite icon = weapon.WeaponData.Sprite;

        Setup(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
    }

    public void Setup(ObjectDataSO objectData, Action clickedCallback)
    {
        ObjectData = objectData;

        Color color = ColorHolder.GetColor(objectData.Rarity);
        Sprite icon = objectData.Icon;

        Setup(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
    }
}
