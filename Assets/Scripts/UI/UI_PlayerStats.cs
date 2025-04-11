using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerStats : MonoBehaviour, IPlayerStatsDependency
{
    [Header(" Elements ")]
    [SerializeField] private Transform playerStatParent;

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        int index = 0;

        foreach(Stat stat in Enum.GetValues(typeof(Stat)))
        {
            UI_StatContainer statContainer = playerStatParent.GetChild(index).GetComponent<UI_StatContainer>();
            statContainer.gameObject.SetActive(true);

            Sprite icon = ResourcesManager.GetStatIcon(stat);
            string statName = Enums.FormatStatName(stat);
            float statValue = playerStatsManager.GetStatValue(stat);

            statContainer.Setup(icon, statName, statValue, true);

            index++;
        }

        for(int i = index; i < playerStatParent.childCount; i++)
            playerStatParent.GetChild(i).gameObject.SetActive(false);
    }
}
