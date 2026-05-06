using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveSystem : MonoBehaviour
{
    // events
    public static event System.Action<int> OnSetCreditBalance;
    public static event System.Action<GameObject> OnNewEnemy;
    public static event System.Action<StartWaveData> OnWaveStarted; // takes gamemode, level num, wave num
    public static event System.Action OnWaveEnded;
    public static event System.Action<LevelEndData> OnLevelEnded; // takes has level ended, level num
    public static event System.Action<int> OnUpdateTimer;

    // variables
    [Header("References")]
    public List<LevelScriptableOject> levels;

    [Header("Enemy Prefabs")]
    public GameObject lightEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject heavyEnemyPrefab;
    public Transform enemySpawnPos;

    [Header("Endless Wave Settings")]
    public int baseEnemyCount;
    public int enemyIncreasePerWave;
    public float baseSpawnDelay;
    public int lightThreshold;
    public int mediumThreshold;

    // private variables
    private GameMode gameMode;
    private int waveNumber;
    private int levelIndex;
    private int endlessWaveDuration;
    private List<EnemyType> endlessEnemiesList = new List<EnemyType>();

    void Awake()
    {
        waveNumber = 1;
    }

    // called by game manager
    public void EndlessMode()
    {
        gameMode = GameMode.Endless;
    }

    // called by game manager
    public void LevelsMode(int index)
    {
        gameMode = GameMode.Levels;
        levelIndex = index;
        OnSetCreditBalance?.Invoke(levels[levelIndex - 1].StartGameBalance);
    }

    // called from UI button
    public void StartNextWave()
    {
        OnWaveStarted?.Invoke(new StartWaveData
        {
            gameMode = gameMode,
            levelNumber = levelIndex,
            waveNumber = waveNumber
        });

        // if endless mode
        if (gameMode == GameMode.Endless)
        {
            GenerateEndlessWave();
            SpawnWave(endlessEnemiesList, endlessWaveDuration);
        }
        // if level mode
        else
        {
            SpawnWave(levels[levelIndex - 1].Waves[waveNumber - 1].SpawnOrder, levels[levelIndex - 1].Waves[waveNumber - 1].WaveDuration);
        }
    }


    private void GenerateEndlessWave()
    {
        // reset ememies
        endlessEnemiesList.Clear();

        // calaculate total enemies 
        int totalEnemies = baseEnemyCount + (waveNumber * enemyIncreasePerWave);
        endlessWaveDuration = (int)(totalEnemies * baseSpawnDelay);

        for (int i = 0; i < totalEnemies; i++)
        {
            if (waveNumber <= lightThreshold)
            {
                endlessEnemiesList.Add(EnemyType.Light);
            }
            else
            {
                if (waveNumber <= mediumThreshold)
                {
                    if (i % 3 == 0)
                    {
                        endlessEnemiesList.Add(EnemyType.Medium);
                    }
                    else
                    {
                        endlessEnemiesList.Add(EnemyType.Light);
                    }
                }
                else
                {
                    if (i % 5 == 0)
                    {
                        endlessEnemiesList.Add(EnemyType.Heavy);
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            endlessEnemiesList.Add(EnemyType.Medium);
                        }
                        else
                        {
                            endlessEnemiesList.Add(EnemyType.Light);
                        }
                    }
                }
            }
        }
    }

    private void SpawnWave(List<EnemyType> enemies, int waveDuration)
    {
        float spawnFreq = (float)waveDuration / (float)enemies.Count;
        StartCoroutine(SpawnEnemies(enemies, spawnFreq));
        StartCoroutine(WaveTimer(waveDuration));
    }
    IEnumerator SpawnEnemies(List<EnemyType> enemies, float spawnFreq)
    {
        foreach (EnemyType enemyType in enemies)
        {
            SpawnEnemy(enemyType);
            yield return new WaitForSeconds(spawnFreq);
        }
    }
    private void SpawnEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Light:
                GameObject lightEnemy = Instantiate(lightEnemyPrefab, enemySpawnPos);
                OnNewEnemy?.Invoke(lightEnemy);
                break;

            case EnemyType.Medium:
                GameObject mediumEnemy = Instantiate(mediumEnemyPrefab, enemySpawnPos);
                OnNewEnemy?.Invoke(mediumEnemy);
                break;

            case EnemyType.Heavy:
                GameObject heavyEnemy = Instantiate(heavyEnemyPrefab, enemySpawnPos);
                OnNewEnemy?.Invoke(heavyEnemy);
                break;
        }
    }

    private IEnumerator WaveTimer(int waveDuration)
    {
        int timer = waveDuration;

        while (timer > 0)
        {
            timer -= 1;
            OnUpdateTimer?.Invoke(timer);
            yield return new WaitForSeconds(1);
        }
    }

    private void IncreaseWaveOrLevel()
    {
        if (gameMode == GameMode.Endless)
        {
            HandleEndlessWaveComplete();
        }
        else
        {
            HandleLevelWaveComplete();
        }
    }

    private void HandleEndlessWaveComplete()
    {
        waveNumber++;
        OnWaveEnded?.Invoke();
    }

    private void HandleLevelWaveComplete()
    {
        // check if there is another wave
        if (waveNumber < levels[levelIndex - 1].Waves.Count)
        {
            // goes to next wave
            waveNumber++;
            OnWaveEnded?.Invoke();
        }
        else
        {
            Debug.Log("No More Waves in current Level");
            // levels mode
            OnLevelEnded?.Invoke(new LevelEndData
            {
                HasNextLevel = levelIndex < levels.Count,
                LevelNumber = levelIndex
            });

            // check if there is another level
            if (levelIndex < levels.Count)
            {
                // reset wave number
                waveNumber = 1;

                levelIndex++;

                // reset the balance to the new level's start balance
                OnSetCreditBalance?.Invoke(levels[levelIndex - 1].StartGameBalance);
            }
        }
    }


    private void OnEnable()
    {
        EnemyManager.OnAllEnemiesCleared += IncreaseWaveOrLevel;
    }

    private void OnDisable()
    {
        EnemyManager.OnAllEnemiesCleared -= IncreaseWaveOrLevel;
    }
}