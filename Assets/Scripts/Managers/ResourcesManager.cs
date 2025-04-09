using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    const string STAT_ICONS_DATA_PATH = "Data/Stat Icons";

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
}
