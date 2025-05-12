using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSelectionButton : MonoBehaviour
{
    [field: SerializeField] public Button Button {  get; private set; }

    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header(" Stats ")]
    [SerializeField] private Transform statContainersParent;


    [Header(" Color ")]
    [SerializeField] private Image btnImage;
    [SerializeField] private Image btnOutline;

    public void Setup(WeaponDataSO weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (Lv {level + 1})"; 

        nameText.color = ColorHolder.GetColor(level);
        btnImage.color = ColorHolder.GetColor(level);
        btnOutline.color = ColorHolder.GetOutlineColor(level);

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        SetupStatContainers(calculatedStats);
    }

    private void SetupStatContainers(Dictionary<Stat,float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
    }

    public void Select()
    {
        AudioManager.instance.PlaySFX(9);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f).setEase(LeanTweenType.easeOutSine);
    }

    public void DeSelect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f);
    }
}
