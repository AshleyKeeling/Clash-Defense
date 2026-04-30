using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EnemyWaveSystem : MonoBehaviour
{
    [Header("References")]
    public List<LevelScriptableOject> levels;
    public GameObject lightEnemyPrefab;
    public GameObject meduimEnemyPrefab;
    public GameObject heavyEnemyPrefab;
    public Transform enemySpawnPos;

    [Header("Endless Wave Settings")]
    public int firstWaveDuration = 3;
    public float enemySpawnFreqInSeconds = 5f;

    public int waveNumber;
    // private variables
    private UIManager uIManager;
    private EnemyManager enemyManager;

    public bool isWaveActive = false;
    public bool startWave = false;

    private GameMode gameMode;
    private int levelIndex;
    private int enemyWaveIndex = 0;

    void Start()
    {
        startWave = false;
        waveNumber = 1;
        uIManager = FindObjectOfType<UIManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    public void EndlessMode()
    {
        gameMode = GameMode.Endless;
    }

    public void LevelsMode(int index)
    {
        gameMode = GameMode.Levels;
        levelIndex = index;
    }

    // called from UI button
    public void StartNextWave()
    {
        // if endless mode
        if (gameMode == GameMode.Endless)
        {
            Debug.Log("Endless Mode");
        }
        else
        {
            Debug.Log("Level Mode");
            Debug.Log(levels[levelIndex - 1].waves[waveNumber - 1].WaveDuration);

            SpawnLevelWave();

        }
        // spawn next wave

        // if levels mode
        // move onto next wave
        // if its last wave in level
        // move onto next level
    }
    private void SpawnLevelWave()
    {
        isWaveActive = true;
        // calculate enemies spawn frequency (duration/enemyCount)
        float spawnFreq = levels[levelIndex - 1].waves[waveNumber - 1].WaveDuration / levels[levelIndex - 1].waves[waveNumber - 1].SpawnOrder.Count;
        // spawn enemy every x frequency working through list
        StartCoroutine(SpawnEnemies(spawnFreq));

        uIManager.DisplayCombatUI();
        uIManager.UpdateWaveNumber(waveNumber);
        StartCoroutine(WaveTimer(levels[levelIndex - 1].waves[waveNumber - 1].WaveDuration));
    }

    private void SpawnEnemy()
    {
        // random enemy
        // int randomIndex = Random.Range(0, 3);

        EnemyType enemyType = levels[levelIndex - 1].waves[waveNumber - 1].SpawnOrder[enemyWaveIndex];

        switch (enemyType)
        {
            case EnemyType.Light:
                GameObject lightEnemy = Instantiate(lightEnemyPrefab, enemySpawnPos);
                enemyManager.AddEnemy(lightEnemy);
                enemyWaveIndex++;
                break;
            case EnemyType.Meduim:
                GameObject meduimEnemy = Instantiate(meduimEnemyPrefab, enemySpawnPos);
                enemyManager.AddEnemy(meduimEnemy);
                enemyWaveIndex++;
                break;
            case EnemyType.Heavy:
                GameObject heavyEnemy = Instantiate(heavyEnemyPrefab, enemySpawnPos);
                enemyManager.AddEnemy(heavyEnemy);
                enemyWaveIndex++;
                break;
        }
    }

    IEnumerator SpawnEnemies(float spawnFreq)
    {
        while (enemyWaveIndex < levels[levelIndex - 1].waves[waveNumber - 1].SpawnOrder.Count)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnFreq);
        }

        StartCoroutine(WaitForEnimiesToClear());

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

            waveNumber++;
            enemyWaveIndex = 0;
            uIManager.DisplayBuildUI();
        }

    }

    private IEnumerator WaveTimer(int waveDuration)
    {
        Debug.Log("StartWave ");

        int timer = waveDuration;

        while (timer > 0)
        {
            timer -= 1;
            uIManager.UpdateWaveTimeDisplay(timer);
            yield return new WaitForSeconds(1);
        }

        // ends wave
        // isWaveActive = false;
        // EndWave();
        Debug.Log("end wave");
    }
}
