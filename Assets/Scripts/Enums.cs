
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
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalPercent,
    MoveSpeed,
    MaxHealth,
    Range,
    Luck
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        string formatted = "";
        string unformat = stat.ToString();

        if (unformat.Length <= 0)
            return "";

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