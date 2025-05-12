using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItemInfo : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI recyclePriceText;

    [Header(" Color ")]
    [SerializeField] private Image container;

    [Header(" Stats ")]
    [SerializeField] private Transform statsParent;

    [field: Header(" Buttons ")]
    [field: SerializeField] public Button RecycleButton { get; private set; }
    [SerializeField] private Button mergeButton;

    private void Setup(Sprite itemIcon, string name, Color containerColor, int recyclePrice, Dictionary<Stat,float> statsDic)
    {
        this.icon.sprite = itemIcon;
        itemNameText.text = name;   
        itemNameText.color = containerColor;

        recyclePriceText.text = recyclePrice.ToString();

        container.color = containerColor;

        StatContainerManager.GenerateStatContainers(statsDic, statsParent);
    }

    public void Setup(Weapon weapon)
    {
        string maxlevel = weapon.Level + 1 < 4 ? "" : "(Max)"; 
        Setup
            (weapon.WeaponData.Sprite,
            weapon.WeaponData.Name + $"(Lv {weapon.Level + 1}{maxlevel})",
            ColorHolder.GetColor(weapon.Level),
            WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level)
            );

        mergeButton.gameObject.SetActive(true);

        mergeButton.interactable = WeaponMerger.instance.CanMerge(weapon);

        mergeButton.onClick.RemoveAllListeners();
        mergeButton.onClick.AddListener(WeaponMerger.instance.Merge);
    }

    public void Setup(ObjectDataSO objectData)
    {
        Setup
            (objectData.Icon,
            objectData.Name,
            ColorHolder.GetColor(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.BaseStats
            );

        mergeButton.gameObject.SetActive(false);
    }
}
