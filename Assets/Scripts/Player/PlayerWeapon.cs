using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private WeaponPosition[] weaponPosition;

    public void AddWeapon(WeaponDataSO selectedWeapon, int weaponLevel)
    {
        weaponPosition[Random.Range(0, weaponPosition.Length)].AssignWeapon(selectedWeapon.Prefab, weaponLevel);
    }
}
