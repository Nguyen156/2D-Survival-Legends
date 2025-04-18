using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    const string STAT_ICONS_DATA_PATH = "Data/Stat Icons";
    const string OBJECT_DATA_PATH = "Data/Objects/";
    const string WEAPON_DATA_PATH = "Data/Weapons/";
    const string CHARACTER_DATA_PATH = "Data/Characters/";

    private static StatIcon[] statIcons;

    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            StatIconDataSO data = Resources.Load<StatIconDataSO>(STAT_ICONS_DATA_PATH);
            statIcons = data.StatIcons;
        }

        foreach (var statIcon in statIcons)
        {
            if (stat == statIcon.stat)
                return statIcon.icon;
        }

        return null;
    }

    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get
        {
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(OBJECT_DATA_PATH);

            return objectDatas;
        }

        private set { }
    }

    public static ObjectDataSO GetRandomObject()
    {
        return Objects[Random.Range(0, Objects.Length)];
    }

    private static WeaponDataSO[] weaponDatas;
    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (weaponDatas == null)
                weaponDatas = Resources.LoadAll<WeaponDataSO>(WEAPON_DATA_PATH);

            return weaponDatas;
        }

        private set { }
    }

    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }

    private static CharacterDataSO[] characterDatas;
    public static CharacterDataSO[] Characters
    {
        get
        {
            if (characterDatas == null)
                characterDatas = Resources.LoadAll<CharacterDataSO>(CHARACTER_DATA_PATH);

            return characterDatas;
        }

        private set { }
    }
}
