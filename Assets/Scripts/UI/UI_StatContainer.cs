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

    public void Setup(Sprite icon, string statName, float statValue, bool useColor = false)
    {
        statImg.sprite = icon;
        statText.text = statName;

        statValueText.color = useColor ? ColorStatValue(statValue) : Color.white;
        statValueText.text = (Mathf.RoundToInt(statValue)).ToString();
    }

    private Color ColorStatValue(float statValue)
    {
        float sign = Mathf.Sign(statValue);

        if (statValue == 0)
            sign = 0;

        Color statValueTextColor = Color.white;

        if (sign > 0)
            statValueTextColor = Color.green;
        else if (sign < 0)
            statValueTextColor = Color.red;

        return statValueTextColor;
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
