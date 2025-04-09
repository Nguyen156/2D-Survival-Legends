using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image statImg;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Setup(Sprite icon, string statName, string statValue)
    {
        statImg.sprite = icon;
        statText.text = statName;
        statValueText.text = statValue;
    }

    public float GetFontSize()
    {
        return statText.fontSize;
    }

    public void SetFontSize(float newfontSize)
    {
        statText.fontSizeMax = newfontSize;
        statValueText.fontSizeMax = newfontSize;
    }
}
