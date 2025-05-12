using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageCompleteManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private int minReward, maxReward;

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.STAGECOMPLETE)
            Setup();
    }

    private void Setup()
    {
        AudioManager.instance.PlaySFX(12, false);

        int reward = Random.Range(minReward, maxReward);
        rewardText.text = "Reward: " + reward;

        CurrencyManager.instance.AddPremiumCurrency(reward);
    }
}
