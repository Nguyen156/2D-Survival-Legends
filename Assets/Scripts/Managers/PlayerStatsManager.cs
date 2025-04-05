using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;

    [Header(" Data ")]
    [SerializeField] private CharacterDataSO playerData;

    [Header(" Settings ")]
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerStats = playerData.BaseStats;

        foreach(KeyValuePair<Stat, float> kvp in playerStats)
            addends.Add(kvp.Key, 0);
    }

    private void Start()
    {
        UpdatePlayerStats();
    }

    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
            addends[stat] += value;
        else
            Debug.LogError($"The key {stat} has not been found, Review your code !!!");

        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];

        return value;
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IPlayerStatsDependency>();

        foreach (var element in playerStatsDependencies)
            element.UpdateStats(this);
    }
}
