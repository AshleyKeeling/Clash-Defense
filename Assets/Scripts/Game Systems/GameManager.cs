using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public EnemyWaveSystem enemyWaveSystem;
    public UIManager uIManager;
    public bool IsPaused;
    private bool IsGameOver;

    private void Start()
    {
        enemyWaveSystem = FindObjectOfType<EnemyWaveSystem>();
        uIManager = FindObjectOfType<UIManager>();
        uIManager.StartGameUISetup();
        IsPaused = false;
        IsGameOver = false;
        Time.timeScale = 1f;
    }
    // Game Over
    public void GameOver()
    {
        IsGameOver = true;

        Debug.Log("Game Over!!!!");

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

            // switches to pause menu UI
            uIManager.EnablePauseMenuUI();

            // pauses time
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        IsPaused = false;

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
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
