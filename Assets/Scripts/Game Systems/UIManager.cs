using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : MonoBehaviour
{
    [Header("Combat UI")]
    public GameObject combatPanel;
    public TextMeshProUGUI currencyNumber1;
    public TextMeshProUGUI waveTimeLeft;
    public TextMeshProUGUI levelNumber;
    public TextMeshProUGUI waveNumber;
    public Button slowSpeedButton;
    public Button normalSpeedButton;
    public Button fastSpeedButton;
    public Button freezeButton;
    public Button damageButton;
    public Button boostButton;

    [Header("Build UI")]
    public GameObject buildPanel;
    public TextMeshProUGUI currencyNumber2;

    [Header("Level Complete UI")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI levelCompletePanel_levelNumber;

    [Header("All Levels Complete UI")]
    public GameObject allLevelsCompletePanel;
    public TextMeshProUGUI allLevelsCompletPanel_levelNumber;
    [Header("Pause Menu UI")]
    public GameObject pauseMenuPanel;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Player Base UI")]
    public Slider PlayerBaseHealth1;
    public Slider PlayerBaseHealth2;

    private bool IsGameInCombatPhase;

    // general functions

    public void StartGameUISetup()
    {
        combatPanel.SetActive(false);
        buildPanel.SetActive(true);
        levelCompletePanel.SetActive(false);
        allLevelsCompletePanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void DisplayCombatUI()
    {
        if (!gameOverPanel.activeSelf)
        {
            combatPanel.SetActive(true);
            buildPanel.SetActive(false);
        }

    }

    public void DisplayBuildUI()
    {
        if (!gameOverPanel.activeSelf)
        {
            buildPanel.SetActive(true);
            levelCompletePanel.SetActive(false);
            combatPanel.SetActive(false);
        }
    }

    public void DisplayLevelCompleteUI()
    {
        if (!gameOverPanel.activeSelf)
        {
            levelCompletePanel.SetActive(true);
            combatPanel.SetActive(false);
        }
    }

    public void DisplayAllLevelsCompleteUI()
    {
        if (!gameOverPanel.activeSelf)
        {
            allLevelsCompletePanel.SetActive(true);
            combatPanel.SetActive(false);
        }
    }

    public void EnablePauseMenuUI()
    {
        // saves current game phase(eihter build or combat)
        if (combatPanel.activeSelf)
        {
            IsGameInCombatPhase = true;
            combatPanel.SetActive(false);
        }
        else
        {
            IsGameInCombatPhase = false;
            buildPanel.SetActive(false);

        }
        pauseMenuPanel.SetActive(true);
    }

    public void DisablePauseMenuUI()
    {
        pauseMenuPanel.SetActive(false);

        if (IsGameInCombatPhase)
        {
            combatPanel.SetActive(true);
        }
        else
        {
            buildPanel.SetActive(true);
        }
    }

    public void DisplayGameOverUI()
    {
        buildPanel.SetActive(false);
        combatPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    // --- combat UI ---
    public void UpdateCurrencyDisplay(int currency)
    {
        currencyNumber1.text = "₡" + currency.ToString() + " credits";
        currencyNumber2.text = "₡" + currency.ToString() + " credits";
    }

    public void UpdateWaveTimeDisplay(int timeLeft)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);

        waveTimeLeft.text = "Time Left: " + time.ToString(@"mm\:ss");
    }

    public void UpdateLevelNumber(int levelNum)
    {
        if (levelNum == 0)
        {
            levelNumber.text = "Endless";
        }
        else
        {
            levelNumber.text = "Level: " + levelNum.ToString();
        }
    }

    public void UpdateWaveNumber(int waveNum)
    {
        waveNumber.text = "Wave: " + waveNum.ToString();
    }

    public void DisableSlowSpeedButton()
    {
        slowSpeedButton.interactable = false;
        normalSpeedButton.interactable = true;
        fastSpeedButton.interactable = true;
    }

    public void DisableNormalSpeedButton()
    {
        slowSpeedButton.interactable = true;
        normalSpeedButton.interactable = false;
        fastSpeedButton.interactable = true;
    }

    public void DisableFastSpeedButton()
    {
        slowSpeedButton.interactable = true;
        normalSpeedButton.interactable = true;
        fastSpeedButton.interactable = false;
    }

    public void SetFreezeButtonState(bool state)
    {
        freezeButton.interactable = state;
    }

    public void SetDamageButtonState(bool state)
    {
        damageButton.interactable = state;
    }

    public void SetBoostButtonState(bool state)
    {
        boostButton.interactable = state;
    }

    // --- build UI ---


    // --- player base UI ---
    public void UpdatePlayerBaseHealthBar(float health, float maxHealth)
    {
        PlayerBaseHealth1.value = health / maxHealth;
        PlayerBaseHealth2.value = health / maxHealth;
    }
}
