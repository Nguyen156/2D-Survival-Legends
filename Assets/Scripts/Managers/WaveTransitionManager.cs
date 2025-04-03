using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private Button[] upgradeButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainer();
                break;
        }
    }

    private void ConfigureUpgradeContainer()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);

            string randomStatString = Enums.FormatStatName((Stat)randomIndex);

            upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = randomStatString;

            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => Debug.Log(randomStatString));
        }
    }
}
