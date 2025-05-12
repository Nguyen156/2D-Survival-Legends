using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(UI_WaveManager))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    public static WaveManager instance;

    [Header(" Elements ")]
    [SerializeField] private Player player;
    private UI_WaveManager ui;

    [Header(" Settings ")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;

    [Header(" Waves ")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    [Header(" Actions ")]
    public static Action OnWaveCompleted;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        ui = GetComponent<UI_WaveManager>();

        Enemy.OnBossDeath += BossDeathCallback;
    }

    private void OnDestroy()
    {
        Enemy.OnBossDeath -= BossDeathCallback;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(!isTimerOn)
            return;

        if (timer < waveDuration)
        {
            //ui.ChangeTimerTextColor((int)waveDuration - (int)timer);
            int timeLeft = Mathf.CeilToInt(waveDuration - (timer + Time.deltaTime));
            ui.UpdatTimerText($"{timeLeft}");

            ui.ChangeTimerTextColor(timeLeft);

            ManageWave();
        }
        else
        {
            isTimerOn = false;
            DefeatAllEnemies();

            AudioManager.instance.PlaySFX(8, false);
            ui.UpdateWaveCompltedText("WAVE COMPLETED!");

            OnWaveCompleted?.Invoke();

            Invoke(nameof(StartWaveTransition), 2f);
        }
    }


    private void StartWave(int waveIndex)
    {
        Debug.Log("Start wave: " + waveIndex);
        ui.UpdateWaveCompltedText("");
        ui.UpdateWaveText($"Wave {currentWaveIndex + 1} / {waves.Length}");

        localCounters.Clear();

        foreach(var segment in waves[waveIndex].segments)
            localCounters.Add(0);

        timer = 0;
        isTimerOn = true;
    }
    private void StartWaveTransition()
    {
        isTimerOn = false;

        DefeatAllEnemies();

        currentWaveIndex++;
        waveDuration += 5;

        if (currentWaveIndex >= waves.Length)
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
        else
            GameManager.instance.WaveCompletedCallback();
    }
    private void DefeatAllEnemies()
    {
        while(transform.childCount > 0)
        {
            Transform enemyToDelete = transform.GetChild(0);
            enemyToDelete.transform.parent = null;
            Destroy(enemyToDelete.gameObject);
        }
    }
    private void ManageWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for(int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStart / 100 * waveDuration;
            float tEnd = segment.tEnd / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequency;

            if(timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPos(), Quaternion.identity, transform);
                localCounters[i]++;

                if (segment.spawnBoss)
                {
                    //localCounters[i] += Mathf.Infinity;
                    isTimerOn = false;
                    ui.UpdatTimerText("BOSS");
                }
            }
        }

        timer += Time.deltaTime;
    }
    private Vector2 GetSpawnPos()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(10, 16);
        Vector2 targetPos = (Vector2)player.transform.position + offset;

        targetPos.x = Mathf.Clamp(targetPos.x, -Constants.arenaSize.x / 2, Constants.arenaSize.x / 2);
        targetPos.y = Mathf.Clamp(targetPos.y, -Constants.arenaSize.y / 2, Constants.arenaSize.y / 2);

        return targetPos;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartWave(currentWaveIndex);
                break;

            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemies(); 
                break;
        }
    }

    private void BossDeathCallback(Vector2 enemyPos)
    {
        StartWaveTransition();
    }

    public int GetCurrentWaveIndex() => currentWaveIndex;
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    public float tStart;
    public float tEnd;
    public float spawnFrequency;
    public GameObject prefab;
    public bool spawnBoss;
}
