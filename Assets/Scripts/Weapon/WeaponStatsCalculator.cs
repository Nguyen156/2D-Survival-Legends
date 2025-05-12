using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat,float> GetStats(WeaponDataSO weaponData, int level)
    {
        //float multiplier = 1 + (float)level / 3;
        float multiplier = (Mathf.Pow(1.5f, level));

        Dictionary<Stat,float> calculatedStats = new Dictionary<Stat,float>();

        foreach(KeyValuePair<Stat,float> kvp in weaponData.BaseStats)
        {
            if (weaponData.Prefab.GetType() != typeof(RangeWeapon) && kvp.Key == Stat.Range || kvp.Key == Stat.CriticalPercent)
                calculatedStats.Add(kvp.Key, kvp.Value);
            else
                calculatedStats.Add(kvp.Key, kvp.Value * multiplier);
        }

        return calculatedStats;
    }

    public static int GetPurchasePrice(WeaponDataSO weaponData, int level)
    {
        float multiplier = Mathf.Pow(2, level);

        return (int)(weaponData.Price * multiplier);
    }

    public static int GetRecyclePrice(WeaponDataSO weaponData, int level)
    {
        int recyclePrice = Mathf.FloorToInt(GetPurchasePrice(weaponData, level) * 0.9f);
        return recyclePrice;
    }
}
