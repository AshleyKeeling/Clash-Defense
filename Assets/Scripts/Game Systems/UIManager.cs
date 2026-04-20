using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class UIManager : MonoBehaviour
{
    [Header("Combat UI")]
    public Canvas combatUI;
    public TextMeshProUGUI currencyNumber1;
    public TextMeshProUGUI waveTimeLeft;
    public TextMeshProUGUI waveNumber;
    public Button slowSpeedButton;
    public Button normalSpeedButton;
    public Button fastSpeedButton;
    public Button freezeButton;
    public Button damageButton;
    public Button boostButton;

    [Header("Build UI")]
    public Canvas buildUI;
    public TextMeshProUGUI currencyNumber2;

    [Header("Game Over UI")]
    public Canvas gameOverUI;

    [Header("Pause Menu UI")]
    public Canvas pauseMenuUI;

    [Header("Player Base UI")]
    public Slider PlayerBaseHealth1;
    public Slider PlayerBaseHealth2;

    private bool IsGameInCombatPhase;

    // general functions

    public void StartGameUISetup()
    {
        combatUI.enabled = false;
        buildUI.enabled = true;
        gameOverUI.enabled = false;
        pauseMenuUI.enabled = false;
    }



    public void DisplayCombatUI()
    {
        if (gameOverUI.enabled != true)
        {
            combatUI.enabled = true;
            buildUI.enabled = false;
        }

    }

    public void DisplayBuildUI()
    {
        if (gameOverUI.enabled != true)
        {
            buildUI.enabled = true;
            combatUI.enabled = false;
        }
    }

    public void DisplayGameOverUI()
    {
        buildUI.enabled = false;
        combatUI.enabled = false;
        gameOverUI.enabled = true;
    }

    public void EnablePauseMenuUI()
    {
        // saves current game phase(eihter build or combat)
        if (combatUI.enabled == true)
        {
            IsGameInCombatPhase = true;
            combatUI.enabled = false;

        }
        else
        {
            IsGameInCombatPhase = false;
            buildUI.enabled = false;

        }

        pauseMenuUI.enabled = true;
    }

    public void DisablePauseMenuUI()
    {
        pauseMenuUI.enabled = false;

        if (IsGameInCombatPhase)
        {
            combatUI.enabled = true;
        }
        else
        {
            buildUI.enabled = true;
        }
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
