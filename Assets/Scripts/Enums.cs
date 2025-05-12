
public enum GameState
{
    MENU,
    WEAPONSELECTION,
    GAME,
    GAMEOVER,
    STAGECOMPLETE,
    WAVETRANSITION,
    SHOP
}

public enum Stat
{
    Damage,
    AttackSpeed,
    CriticalChance,
    CriticalPercent,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRecoverySpeed,
    Armor,
    Dodge,
    LifeSteal,
}

public static class Enums
{
    public static string FormatStatName(Stat stat, bool usePercent = true)
    {
        string formatted = "";
        string unformat = stat.ToString();

        if (unformat.Length <= 0)
            return "";

        if(usePercent)
        {
            switch (stat)
            {
                case Stat.AttackSpeed:
                    formatted += "%"; 
                    break;

                case Stat.CriticalChance:
                    formatted += "%";
                    break;

                case Stat.MoveSpeed:
                    formatted += "%";
                    break;

                case Stat.Dodge:
                    formatted += "%";
                    break;

                case Stat.LifeSteal:
                    formatted += "%";
                    break;
            }
        }

        formatted += unformat[0];

        for(int i = 1; i < unformat.Length; i++)
        {
            if(char.IsUpper(unformat[i]))
                formatted += " ";

            formatted += unformat[i];
        }

        return formatted;
    }
}