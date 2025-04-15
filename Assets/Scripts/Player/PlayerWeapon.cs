using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private WeaponPosition[] weaponPositions;

    public bool TryAddWeapon(WeaponDataSO weaponData, int weaponLevel)
    {
        for(int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null)
                continue;

            weaponPositions[i].AssignWeapon(weaponData.Prefab, weaponLevel);
            return true;
        }

        return false;
    }

    public void RemoveWeapon(int weaponIndex)
    {
        int recyclePrice = weaponPositions[weaponIndex].Weapon.GetRecyclePrice();
        CurrencyManager.instance.AddCurrency(recyclePrice);

        weaponPositions[weaponIndex].RemoveWeapon();
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weaponList = new List<Weapon>();

        foreach(var weaponPos in weaponPositions)
        {
            if (weaponPos.Weapon == null)
                weaponList.Add(null);
            else 
                weaponList.Add(weaponPos.Weapon);

        }

        return weaponList.ToArray();
    }
}
