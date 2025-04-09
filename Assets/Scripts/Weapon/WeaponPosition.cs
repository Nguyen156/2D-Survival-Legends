using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    public Weapon Weapon {  get; private set; }

    public void AssignWeapon(Weapon weapon, int weaponLevel)
    {
        Weapon = Instantiate(weapon, transform);

        Weapon.transform.localPosition = Vector2.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.UpgradeTo(weaponLevel);
    }
}
