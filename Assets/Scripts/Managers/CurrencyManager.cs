using System;
using System.Collections;
using System.Collections.Generic;
using Nguyen.SaveSystem;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency {  get; private set; }
    [field: SerializeField] public int PremiumCurrency {  get; private set; }

    [Header(" Actions ")]
    public static Action OnUpdated;

    [Header(" Keys ")]
    private const string PREMIUM_CURRENCY_KEY = "PremiumCurrency";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Candy.OnCollected += CandyCollectedCallback;
        Cash.OnCollected += CashCollectedCallback;
    }

    private void OnDestroy()
    {
        Candy.OnCollected -= CandyCollectedCallback;
        Cash.OnCollected -= CashCollectedCallback;

    }

    public void Load()
    {
        if (SaveSystem.TryLoad(this, PREMIUM_CURRENCY_KEY, out object premiumCurrencyValue))
            AddPremiumCurrency((int)premiumCurrencyValue);
        else
            AddPremiumCurrency(100);
    }

    public void Save()
    {
        SaveSystem.Save(this, PREMIUM_CURRENCY_KEY, PremiumCurrency);
    }

    private void Start()
    {
        UpdateTexts();
    }

    [NaughtyAttributes.Button]
    private void Add500Currency() => AddCurrency(500);

    [NaughtyAttributes.Button]
    private void Add500PremiumCurrency() => AddPremiumCurrency(500);

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateUI();
    }

    public void AddPremiumCurrency(int amount)
    {
        PremiumCurrency += amount;
        UpdateUI();
    }

    public void UseCurrency(int price) => AddCurrency(-price);
    public void UsePremiumCurrency(int price) => AddPremiumCurrency(-price);

    private void UpdateUI()
    {
        UpdateTexts();

        OnUpdated?.Invoke();

        Save();
    }

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var child in currencyTexts)
            child.UpdateText(Currency);

        PremiumCurrencyText[] premiumCurrencyTexts = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var child in premiumCurrencyTexts)
            child.UpdateText(PremiumCurrency);
    }

    public bool HasEnoughCurrency(int price) => Currency >= price;
    public bool HasEnoughPremiumCurrency(int price) => PremiumCurrency >= price;

    private void CandyCollectedCallback(Candy candy) => AddCurrency(1);

    private void CashCollectedCallback(Cash cash) => AddPremiumCurrency(1);

    
}
