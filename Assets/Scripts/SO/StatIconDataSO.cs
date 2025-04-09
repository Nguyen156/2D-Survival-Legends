using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Icons", menuName = "Scriptable Objects/Stat Icons")]
public class StatIconDataSO : ScriptableObject
{
    [field: SerializeField] public StatIcon[] StatIcons { get; private set; }
}

[Serializable]
public struct StatIcon
{
    public Stat stat;
    public Sprite icon;
}
