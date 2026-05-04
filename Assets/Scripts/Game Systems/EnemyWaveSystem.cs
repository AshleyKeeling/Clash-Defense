using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveSystem : MonoBehaviour
{
    // events
    public static event System.Action<int> OnSetCreditBalance;
    public static event System.Action<GameObject> OnNewEnemy;
    public static event System.Action<int, int> OnStartWave; // takes level num, wave num
    public static event System.Action OnEndWave;
    public static event System.Action<bool, int> OnLevelEnded; // takes has level ended, level num
    public static event System.Action<int> OnUpdateTimer;

    // variables
    [Header("References")]
    public List<LevelScriptableOject> levels;

    [Header("Enemy Prefabs")]
    public GameObject lightEnemyPrefab;
    public GameObject meduimEnemyPrefab;
    public GameObject heavyEnemyPrefab;
    public Transform enemySpawnPos;

    [Header("Endless Wave Settings")]
    public int baseEnemyCount;
    public int enemyIncreasePerWave;
    public float baseSpawnDelay;
    public int lightThreshold;
    public int meduimThreshold;

    // private variables
    private GameMode gameMode;
    private int waveNumber;
    private int levelIndex;
    private int enemyWaveIndex = 0;
    private int endlessWaveDuration;
    private bool isWaveActive = false;
    private bool hasNextLevel;
    private List<EnemyType> endlessEnemiesList = new List<EnemyType>();

    void Start()
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
        // if endless mode
        if (gameMode == GameMode.Endless)
        {
            GenerateEndlessWave();
            SpawnWave(endlessEnemiesList, endlessWaveDuration);
            OnStartWave?.Invoke(0, waveNumber);
        }
        else
        {
            SpawnWave(levels[levelIndex - 1].Waves[waveNumber - 1].SpawnOrder, levels[levelIndex - 1].Waves[waveNumber - 1].WaveDuration);
            OnStartWave?.Invoke(levelIndex, waveNumber);
        }
    }


    private void GenerateEndlessWave()
    {
        // reset ememies
        endlessEnemiesList.Clear();
        isWaveActive = true;

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
                if (waveNumber <= meduimThreshold)
                {
                    if (i % 3 == 0)
                    {
                        endlessEnemiesList.Add(EnemyType.Meduim);
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
                            endlessEnemiesList.Add(EnemyType.Meduim);
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
        isWaveActive = true;
        float spawnFreq = waveDuration / enemies.Count;
        StartCoroutine(SpawnEnemies(enemies, spawnFreq));
        StartCoroutine(WaveTimer(waveDuration));
    }
    IEnumerator SpawnEnemies(List<EnemyType> enemies, float spawnFreq)
    {
        enemyWaveIndex = 0;

        while (enemyWaveIndex < enemies.Count)
        {
            SpawnEnemy(enemies[enemyWaveIndex]);
            yield return new WaitForSeconds(spawnFreq);
        }

        StartCoroutine(WaitForEnimiesToClear());
    }
    private void SpawnEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Light:
                GameObject lightEnemy = Instantiate(lightEnemyPrefab, enemySpawnPos);
                OnNewEnemy?.Invoke(lightEnemy);
                enemyWaveIndex++;
                break;

            case EnemyType.Meduim:
                GameObject meduimEnemy = Instantiate(meduimEnemyPrefab, enemySpawnPos);
                OnNewEnemy?.Invoke(meduimEnemy);
                enemyWaveIndex++;
                break;

            case EnemyType.Heavy:
                GameObject heavyEnemy = Instantiate(heavyEnemyPrefab, enemySpawnPos);
                OnNewEnemy?.Invoke(heavyEnemy);
                enemyWaveIndex++;
                break;
        }
    }


    private IEnumerator WaitForEnimiesToClear()
    {
        if (isWaveActive)
        {
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                // wait one frame
                yield return null;
            }

            IncreaseWaveOrLevelIndex();
            enemyWaveIndex = 0;
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

    private void IncreaseWaveOrLevelIndex()
    {

        // check if endless or levels mode
        if (gameMode == GameMode.Endless)
        {
            waveNumber++;
        }
        else
        {
            // check if there is another wave
            if (waveNumber < levels[levelIndex - 1].Waves.Count)
            {
                // goes to next wave
                waveNumber++;
                OnEndWave?.Invoke();
            }
            else
            {
                Debug.Log("No More Waves in current Level");

                // check if there is another level
                if (levelIndex < levels.Count)
                {
                    // reset wave number
                    waveNumber = 1;

                    hasNextLevel = true;
                }
                else
                {
                    hasNextLevel = false;
                    Debug.Log("NO MORE LEVELS");
                }
                OnLevelEnded?.Invoke(hasNextLevel, levelIndex);

                if (hasNextLevel)
                {
                    // goes to next level
                    levelIndex++;
                }
            }
        }
    }
}