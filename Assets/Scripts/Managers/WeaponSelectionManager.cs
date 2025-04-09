using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private Transform buttonParent;
    [SerializeField] private UI_WeaponButton prefab;
    [SerializeField] private PlayerWeapon playerWeapon;

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

                playerWeapon.AddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Setup()
    {
        buttonParent.Clear();

        for(int i = 0; i < 3; i++)
            GenerateWeaponSelectButtons();
    }

    private void GenerateWeaponSelectButtons()
    {
        UI_WeaponButton newWeaponButton = Instantiate(prefab, buttonParent);

        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(0, 4);

        newWeaponButton.Setup(weaponData.Sprite, weaponData.Name, level, weaponData);

        newWeaponButton.Button.onClick.RemoveAllListeners();
        newWeaponButton.Button.onClick.AddListener(() => WeaponSelectedCallback(newWeaponButton, weaponData, level));
    }

    private void WeaponSelectedCallback(UI_WeaponButton weaponButton, WeaponDataSO weaponData, int weaponLevel)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = weaponLevel;

        foreach(var child in buttonParent.GetComponentsInChildren<UI_WeaponButton>())
        {
            if (child == weaponButton)
                child.Select();
            else
                child.DeSelect();
        }
    }
}
