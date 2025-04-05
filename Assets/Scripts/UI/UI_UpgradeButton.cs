using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeButton : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeValueText;

    [field: SerializeField] public Button Button {  get; private set; }

    public void Configure(Sprite upgradeIcon, string upgradeName, string upgradeValue)
    {
        icon.sprite = upgradeIcon;
        upgradeNameText.text = upgradeName;
        upgradeValueText.text = upgradeValue;
    }
}
