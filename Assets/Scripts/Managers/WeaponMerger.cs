using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger instance;

    [Header(" Elements ")]
    [SerializeField] private PlayerWeapon playerWeapon;

    [Header(" Settings ")]
    private List<Weapon> weaponsToMerge = new List<Weapon>();

    [Header(" Actions ")]
    public static Action<Weapon> OnMerge;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public bool CanMerge(Weapon weapon)
    {
        if(weapon.Level >= 3)
            return false;

        weaponsToMerge.Clear();
        weaponsToMerge.Add(weapon);

        Weapon[] weapons = this.playerWeapon.GetWeapons();

        foreach(var playerWeapon in weapons)
        {
            if (playerWeapon == null)
                continue;

            // Can't merge with itself
            if(playerWeapon == weapon)
                continue;

            //Not the same weapon
            if(playerWeapon.WeaponData.Name != weapon.WeaponData.Name) 
                continue;

            if(playerWeapon.Level != weapon.Level)
                continue;

            weaponsToMerge.Add(playerWeapon);

            return true;
        }

        return false;
    }

    public void Merge()
    {
        if(weaponsToMerge.Count < 2)
            return;

        DestroyImmediate(weaponsToMerge[1].gameObject);

        weaponsToMerge[0].Upgrade();

        Weapon weapon = weaponsToMerge[0];
        weaponsToMerge.Clear();

        OnMerge?.Invoke(weapon);
    }
}
