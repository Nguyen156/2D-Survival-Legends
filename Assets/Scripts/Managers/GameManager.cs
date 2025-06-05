using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using Nguyen.SaveSystem;

public class GameManager : MonoBehaviour, IWantToBeSaved
{
    public static GameManager instance;

    private bool hasProgress = false;

    private const string HAS_PROGRESS_KEY = "HasProgressKey";

    [Header(" Actions ")]
    public static Action OnGamePaused;
    public static Action OnGameResumed;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartMenu();
    }

    public void StartMenu() => SetGameState(GameState.MENU);
    public void Quit() => Application.Quit();
    public void StartGame()
    {
        AudioManager.instance.PlaySFX(9);
        SetGameState(GameState.GAME);
    }

    public void StartWeaponSelection()
    {
        AudioManager.instance.PlaySFX(9);
        SetGameState(GameState.WEAPONSELECTION);
    }

    public void StartShop()
    {
        AudioManager.instance.PlaySFX(9);
        SetGameState(GameState.SHOP);
    }
    public void ReloadScene()
    {
        AudioManager.instance.PlaySFX(9);
        SceneManager.LoadScene(0);
    }

    public void PauseButtonCallback()
    {
        AudioManager.instance.PlaySFX(9);
        Time.timeScale = 0;
        OnGamePaused?.Invoke();
    }

    public void ResumeButtonCallback()
    {
        AudioManager.instance.PlaySFX(9);
        Time.timeScale = 1;
        OnGameResumed?.Invoke();
    }

    public void RestartFromPause()
    {
        AudioManager.instance.PlaySFX(9);
        Time.timeScale = 1;
        ReloadScene();
    }

    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> gameStateListeners = 
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach(var element in gameStateListeners)
            element.GameStateChangedCallback(gameState);
    }

    public void WaveCompletedCallback()
    {
        if (Player.instance.HasLevelUp())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }

    public bool HasProgress() => hasProgress;

    public void SetProgress()
    {
        hasProgress = true;
        Save();
    }

    public void Load()
    {
        if(SaveSystem.TryLoad(this, HAS_PROGRESS_KEY, out object hasProgressObject))
            hasProgress = (bool) hasProgressObject;
    }

    public void Save()
    {
        SaveSystem.Save(this, HAS_PROGRESS_KEY, hasProgress);
    }
}
