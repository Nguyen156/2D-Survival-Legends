using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyText : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int currency)
    {
        if (text == null)
            return;

        text.text = currency.ToString();
    }
}
