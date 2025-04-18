using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PremiumCurrencyText : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int premiumCurrency)
    {
        if (text == null)
            return;

        text.text = premiumCurrency.ToString();
    }
}
