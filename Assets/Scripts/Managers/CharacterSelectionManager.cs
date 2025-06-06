using System;
using System.Collections;
using System.Collections.Generic;
using Nguyen.SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    public static CharacterSelectionManager instance;

    [Header(" Elements ")]
    [SerializeField] private Transform characterButtonsParent;
    [SerializeField] private UI_CharacterButton characterButtonPrefab;
    [SerializeField] private Image centerCharacterImage;
    [SerializeField] private UI_CharacterInfoPanel characterInfo;

    [Header(" Datas ")]
    private CharacterDataSO[] characterDatas;
    private List<bool> unlockedStates = new List<bool>();

    [Header(" Settings ")]
    private int selectedCharacterIndex;
    private int lastSelectedCharacter;

    [Header(" Actions ")]
    public static Action<CharacterDataSO> OnCharacterSelected;

    [Header(" Keys ")]
    private const string UNLOCKED_STATES_KEY = "UnlockedStatesKey"; 
    private const string LAST_SELECTED_CHARACTER_KEY = "LastSelectedCharacterKey"; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        characterInfo.Button.onClick.RemoveAllListeners();
        characterInfo.Button.onClick.AddListener(PurchaseSelctedCharacter);

        CharacterSelectedCallback(lastSelectedCharacter);

    }

    private void Initialize()
    {
        for(int i = 0; i < characterDatas.Length; i++)
            CreateCharacterButton(i);
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];

        UI_CharacterButton newCharacterButton = Instantiate(characterButtonPrefab, characterButtonsParent);
        newCharacterButton.Setup(characterData.Sprite, unlockedStates[index]);

        newCharacterButton.Button.onClick.RemoveAllListeners();
        newCharacterButton.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        //AudioManager.instance.PlaySFX(9);
        selectedCharacterIndex = index;

        CharacterDataSO characterData = characterDatas[index];

        if (unlockedStates[index])
        {
            lastSelectedCharacter = index;
            characterInfo.Button.interactable = false;
            Save();

            OnCharacterSelected?.Invoke(characterData);
        }
        else
            characterInfo.Button.interactable = 
                CurrencyManager.instance.HasEnoughPremiumCurrency(characterData.Price);


        centerCharacterImage.sprite = characterData.Sprite;
        characterInfo.Setup(characterData, unlockedStates[index]);
    }

    private void PurchaseSelctedCharacter()
    {
        int price = characterDatas[selectedCharacterIndex].Price;
        CurrencyManager.instance.UsePremiumCurrency(price);
        AudioManager.instance.PlaySFX(10);

        //Save the unlocked state
        unlockedStates[selectedCharacterIndex] = true;

        //Updtae UI
        characterButtonsParent.GetChild(selectedCharacterIndex).GetComponent<UI_CharacterButton>().Unlock();

        //Update character info
        CharacterSelectedCallback(selectedCharacterIndex);

        Save();
    }

    public void Load()
    {
        characterDatas = ResourcesManager.Characters;

        for (int i = 0; i < characterDatas.Length; i++)
            unlockedStates.Add(i == 0);

        if(SaveSystem.TryLoad(this, UNLOCKED_STATES_KEY, out object unlockedStatesObject))
            unlockedStates = (List<bool>)unlockedStatesObject;

        if (SaveSystem.TryLoad(this, LAST_SELECTED_CHARACTER_KEY, out object lastSelectedCharacterObject))
            lastSelectedCharacter = (int)lastSelectedCharacterObject;

        Initialize();
    }

    public void Save()
    {
        SaveSystem.Save(this, UNLOCKED_STATES_KEY, unlockedStates);
        SaveSystem.Save(this, LAST_SELECTED_CHARACTER_KEY, lastSelectedCharacter);
    }

    public void ResizeCharacterInfo() => CharacterSelectedCallback(lastSelectedCharacter);
}
