using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ChestObjectContainer : MonoBehaviour
{
    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }

    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header(" Stats ")]
    [SerializeField] private Transform statContainersParent;

    [Header(" Color ")]
    [SerializeField] private Image btnImage;
    [SerializeField] private Image btnOutline;

    public void Setup(ObjectDataSO objectData)
    {
        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;
        recyclePriceText.text = objectData.RecyclePrice.ToString();

        nameText.color = ColorHolder.GetColor(objectData.Rarity);
        btnImage.color = ColorHolder.GetColor(objectData.Rarity);
        btnOutline.color = ColorHolder.GetOutlineColor(objectData.Rarity);

        SetupStatContainers(objectData.BaseStats);
    }

    private void SetupStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statContainersParent);
    }

}
