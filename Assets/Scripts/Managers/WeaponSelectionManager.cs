using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private Transform buttonParent;
    [SerializeField] private UI_WeaponSelectionButton prefab;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private Button goButton;


    [Header(" Data ")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WEAPONSELECTION:
                Setup();
                break;

            case GameState.GAME:
                if (selectedWeapon == null)
                    return;

                playerWeapon.TryAddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Setup()
    {
        buttonParent.Clear();
        goButton.interactable = false;

        for(int i = 0; i < 3; i++)
            GenerateWeaponSelectButtons();
    }

    private void GenerateWeaponSelectButtons()
    {
        UI_WeaponSelectionButton newWeaponButton = Instantiate(prefab, buttonParent);

        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        //int level = Random.Range(0, 4);
        int level = 0;

        newWeaponButton.Setup(weaponData, level);

        newWeaponButton.Button.onClick.RemoveAllListeners();
        newWeaponButton.Button.onClick.AddListener(() => WeaponSelectedCallback(newWeaponButton, weaponData, level));
    }

    private void WeaponSelectedCallback(UI_WeaponSelectionButton weaponButton, WeaponDataSO weaponData, int weaponLevel)
    {
        goButton.interactable = true;

        selectedWeapon = weaponData;
        initialWeaponLevel = weaponLevel;

        foreach(var child in buttonParent.GetComponentsInChildren<UI_WeaponSelectionButton>())
        {
            if (child == weaponButton)
                child.Select();
            else
                child.DeSelect();
        }
    }
}
