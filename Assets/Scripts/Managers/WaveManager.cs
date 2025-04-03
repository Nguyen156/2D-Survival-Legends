using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_WaveManager))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
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

    private void Awake()
    {
        ui = GetComponent<UI_WaveManager>();
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
            ui.UpdatTimerText(((int)(waveDuration - timer)).ToString());

            ManageWave();
        }
        else
        {
            StartWaveTransition();
        }
    }


    private void StartWave(int waveIndex)
    {
        Debug.Log("Start wave: " + waveIndex);
        ui.UpdateWaveText($"Wave {currentWaveIndex + 1} / {waves.Length}");

        localCounters.Clear();

        foreach(var segment in waves[waveIndex].segments)
            localCounters.Add(1);

        timer = 0;
        isTimerOn = true;
    }
    private void StartWaveTransition()
    {
        isTimerOn = false;

        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            ui.UpdateWaveText("Victory !!!");
            ui.UpdatTimerText(" ");

            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
        }
        else
            GameManager.instance.WaveCompleted();
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
            }
        }

        timer += Time.deltaTime;
    }
    private Vector2 GetSpawnPos()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(6, 10);

        Vector2 targetPos = (Vector2)player.transform.position + offset;

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
}
