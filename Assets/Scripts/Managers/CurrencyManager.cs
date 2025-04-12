using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency {  get; private set; }

    public static Action OnUpdated;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    [NaughtyAttributes.Button]
    private void Add500Currency() => AddCurrency(500);

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateUI();

        OnUpdated?.Invoke();
    }

    public void UseCurrency(int price) => AddCurrency(-price);

    private void UpdateUI()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var child in currencyTexts)
            child.UpdateText(Currency);
    }

    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }
}
