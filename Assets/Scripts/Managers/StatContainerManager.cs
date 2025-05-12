using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;

    [Header(" Elements ")]
    [SerializeField] private UI_StatContainer statContainerPrefab;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent, bool usePercent = true)
    {
        parent.Clear();
        instance.GenerateContainers(statDictionary, parent, usePercent);
    }

    private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent, bool usePercent)
    {
        List<UI_StatContainer> statContainerList = new List<UI_StatContainer>();

        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            UI_StatContainer newStatContainer = Instantiate(statContainerPrefab, parent);
            statContainerList.Add(newStatContainer);

            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key, usePercent);
            float statValue = Mathf.Round(kvp.Value);

            newStatContainer.Setup(icon, statName, statValue);
        }

        LeanTween.delayedCall(Time.deltaTime * 2 ,() => ResizeTexts(statContainerList));
    }

    private void ResizeTexts(List<UI_StatContainer> statContainerList)
    {
        float minFontSize = 5000;

        for(int i = 0; i < statContainerList.Count; i++)
        {
            UI_StatContainer statContainer = statContainerList[i];

            float fontSize = statContainer.GetFontSize();

            if (fontSize < minFontSize)
                minFontSize = fontSize;
        }

        for(int i = 0; i < statContainerList.Count; i++)
            statContainerList[i].SetFontSize(minFontSize);
    }
}
