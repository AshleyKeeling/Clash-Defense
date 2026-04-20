using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EnemyWaveSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject lightEnemyPrefab;
    public GameObject meduimEnemyPrefab;
    public GameObject heavyEnemyPrefab;
    public Transform enemySpawnPos;

    [Header("Wave Settings")]
    public int firstWaveDuration = 3;
    public float enemySpawnFreqInSeconds = 5f;

    public int waveNumber = 1;
    // private variables
    private UIManager uIManager;
    private EnemyManager enemyManager;

    public bool isWaveActive = false;
    public bool startWave = false;

    void Start()
    {
        startWave = false;
        uIManager = FindObjectOfType<UIManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void StartWave()
    {
        isWaveActive = true;
        // ui code
        uIManager.DisplayCombatUI();
        uIManager.UpdateWaveNumber(waveNumber);
        StartCoroutine(WaveTimer(firstWaveDuration));

        // scales enemy spawn freq to wave number
        enemySpawnFreqInSeconds = Mathf.Max(0.2f, 1f - waveNumber * 0.05f);

        InvokeRepeating("SpawnEnemy", 0f, enemySpawnFreqInSeconds);
    }

    private void EndWave()
    {
        CancelInvoke("SpawnEnemy");

        // wait till there are no remaining enimies
        StartCoroutine(WaitForEnimiesToClear());
    }

    private IEnumerator WaitForEnimiesToClear()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            // wait one frame
            yield return null;
        }

        waveNumber += 1;
        uIManager.DisplayBuildUI();
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
        isWaveActive = false;
        EndWave();
        Debug.Log("end wave");
    }

    private void SpawnEnemy()
    {
        // random enemy
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                GameObject lightEnemy = Instantiate(lightEnemyPrefab, enemySpawnPos);
                enemyManager.AddEnemy(lightEnemy);
                break;
            case 1:
                GameObject meduimEnemy = Instantiate(meduimEnemyPrefab, enemySpawnPos);
                enemyManager.AddEnemy(meduimEnemy);
                break;
            case 2:
                GameObject heavyEnemy = Instantiate(heavyEnemyPrefab, enemySpawnPos);
                enemyManager.AddEnemy(heavyEnemy);
                break;
        }
    }

    public void StartNextWave()
    {

        startWave = true;
    }

    void Update()
    {
        if (startWave == true && isWaveActive == false)
        {
            StartWave();
            startWave = false;
        }
    }
}
