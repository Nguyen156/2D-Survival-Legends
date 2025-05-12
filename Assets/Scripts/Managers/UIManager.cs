using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject restartConfirmationPanel;
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject settingsPanel;

    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            weaponSelectionPanel,
            gamePanel, 
            gameOverPanel,
            stageCompletePanel,
            waveTransitionPanel, 
            shopPanel
        });

        GameManager.OnGamePaused += GamePausedCallback;
        GameManager.OnGameResumed += GameResumedCallback;

        pausePanel.SetActive(false);
        HideRestartConfirmationPanel();
        HideCharacterSelection();

        HideSettings();
    }

    private void OnDestroy()
    {
        GameManager.OnGamePaused -= GamePausedCallback;
        GameManager.OnGameResumed -= GameResumedCallback;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;

            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;

            case GameState.GAME:
                ShowPanel(gamePanel);
                break;

            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;

            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;

            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
        }
    }

    private void ShowPanel(GameObject panelToShow)
    {
        foreach (var p in panels)
            p.SetActive(p == panelToShow);
    }
    private void GamePausedCallback()
    {
        pausePanel.SetActive(true);
    }

    private void GameResumedCallback()
    {
        pausePanel.SetActive(false);
    }

    public void ShowRestartConfirmationPanel()
    {
        AudioManager.instance.PlaySFX(9);
        restartConfirmationPanel.SetActive(true);
    }
    public void HideRestartConfirmationPanel() => restartConfirmationPanel.SetActive(false);

    public void ShowCharacterSelection()
    {
        AudioManager.instance.PlaySFX(9);
        CharacterSelectionManager.instance.ResizeCharacterInfo();
        characterSelectionPanel.SetActive(true);
    }
    public void HideCharacterSelection()
    {
        //AudioManager.instance.PlaySFX(9);
        characterSelectionPanel.SetActive(false); 
    }
    
    public void ShowSettings()
    {
        AudioManager.instance.PlaySFX(9);
        settingsPanel.SetActive(true);
    }
    public void HideSettings()
    {
        //AudioManager.instance.PlaySFX(9);
        settingsPanel.SetActive(false);
    }
}
