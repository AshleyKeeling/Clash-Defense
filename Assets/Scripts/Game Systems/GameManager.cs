using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("Game Prefs")]
    public int endlessModeStartCreditBalance;
    private EnemyWaveSystem enemyWaveSystem;
    private CurrencyManager currencyManager;
    private UIManager uIManager;
    public bool IsPaused;
    private bool IsGameOver;

    private void Awake()
    {
        enemyWaveSystem = FindObjectOfType<EnemyWaveSystem>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        uIManager = FindObjectOfType<UIManager>();
    }
    private void Start()
    {
        if (GameSession.Instance.selectedMode == GameMode.Endless)
        {
            // start endless mode
            enemyWaveSystem.EndlessMode();

            // set start balance
            currencyManager.SetCreditBalance(endlessModeStartCreditBalance);
        }
        else
        {
            // start levels mode
            enemyWaveSystem.LevelsMode(GameSession.Instance.levelIndex);
        }

        uIManager.StartGameUISetup();

        // standard settings
        AudioListener.pause = false;
        IsPaused = false;
        IsGameOver = false;
        Time.timeScale = 1f;
    }
    // Game Over
    public void GameOver()
    {
        IsGameOver = true;

        // disable game systems
        enemyWaveSystem.enabled = false;

        // change UI to game over ui
        uIManager.DisplayGameOverUI();
    }

    public void PauseGame()
    {
        if (!IsGameOver)
        {
            IsPaused = true;

            // pause sound
            AudioListener.pause = true;

            // switches to pause menu UI
            uIManager.EnablePauseMenuUI();

            // pauses time
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        IsPaused = false;

        // un pauses audio
        AudioListener.pause = false;

        // switches to back to game UI
        uIManager.DisablePauseMenuUI();


        // resumes time
        Time.timeScale = 1f;
    }


    public void SlowTimeSpeed()
    {
        Time.timeScale = 0.5f;
        uIManager.DisableSlowSpeedButton();
    }

    public void NormalTimeSpeed()
    {
        Time.timeScale = 1f;
        uIManager.DisableNormalSpeedButton();
    }

    public void FastTimeSpeed()
    {
        Time.timeScale = 1.5f;
        uIManager.DisableFastSpeedButton();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("StartMenu");
        Debug.Log("Game Quit");
    }

    private void OnEnable()
    {
        PlayerBaseHealth.OnPlayerBaseDestroyed += GameOver;
    }

    private void OnDisable()
    {
        PlayerBaseHealth.OnPlayerBaseDestroyed -= GameOver;
    }
}
