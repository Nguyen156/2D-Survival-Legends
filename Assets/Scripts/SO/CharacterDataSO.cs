using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/New Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string Name {  get; private set; } 
    [field: SerializeField] public Sprite Sprite {  get; private set; } 
    [field: SerializeField] public int Price {  get; private set; }

    [Space]

    public float attack;
    public float attackSpeed;
    public float criticalChance;
    public float criticalPercent;
    public float moveSpeed;
    public float maxHealth;
    public float range;
    public float healthRecoverySpeed;
    public float armor;
    public float luck;
    public float dodge;
    public float lifeSteal;

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                {Stat.Attack,              attack},
                {Stat.AttackSpeed,         attackSpeed},
                {Stat.CriticalChance,      criticalChance},
                {Stat.CriticalPercent,     criticalPercent},
                {Stat.MoveSpeed,           moveSpeed},
                {Stat.MaxHealth,           maxHealth},
                {Stat.Range,               range},
                {Stat.HealthRecoverySpeed, healthRecoverySpeed},
                {Stat.Armor,               armor},
                {Stat.Luck,                luck},
                {Stat.Dodge,               dodge},
                {Stat.LifeSteal,           lifeSteal}
            };
        }

        set { }
    }

    public Dictionary<Stat, float> NonNeutralStats
    {
        get
        {
            Dictionary<Stat, float> nonNeutralStats = new Dictionary<Stat, float>();

            foreach (KeyValuePair<Stat,float> kvp in BaseStats)
                if (kvp.Value != 0)
                    nonNeutralStats.Add(kvp.Key, kvp.Value);

            return nonNeutralStats;
        }

        private set { }
    }
}
