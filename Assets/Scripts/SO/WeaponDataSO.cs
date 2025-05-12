using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/New Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public string Name {  get; private set; }
    [field: SerializeField] public Sprite Sprite {  get; private set; }
    [field: SerializeField] public int Price {  get; private set; }
    //[field: SerializeField] public int RecyclePrice {  get; private set; }

    [field: SerializeField] public Weapon Prefab { get; private set; }

    [Space]
    public float damage;
    public float attackSpeed;
    public float criticalChance;
    public float criticalPercent;
    public float range;

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>()
            {
                {Stat.Damage, damage},
                {Stat.AttackSpeed, attackSpeed},
                {Stat.CriticalChance, criticalChance},
                {Stat.CriticalPercent, criticalPercent},
                {Stat.Range, range}
            };
        }

        private set { }
    }

    public float GetStatValue(Stat stat)
    {
        if (BaseStats.ContainsKey(stat))
            return BaseStats[stat];

        return 0;
    }
}
