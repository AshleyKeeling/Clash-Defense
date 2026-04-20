using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private UIManager uIManager;
    public int startGameBalance;

    private int playerCreditsBalance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();

        playerCreditsBalance = startGameBalance;
        uIManager.UpdateCurrencyDisplay(playerCreditsBalance);
    }

    public void AddCredits(int amount)
    {
        playerCreditsBalance += amount;
        uIManager.UpdateCurrencyDisplay(playerCreditsBalance);
    }

    public void SubtractCredits(int amount)
    {
        playerCreditsBalance -= amount;
        uIManager.UpdateCurrencyDisplay(playerCreditsBalance);
    }

    public bool CanPlayerAfford(int cost)
    {
        if (playerCreditsBalance - cost >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
