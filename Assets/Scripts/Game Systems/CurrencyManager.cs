using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    // events
    public static event System.Action<int> OnCurrencyUpdate;

    // variables
    private int playerCreditsBalance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnCurrencyUpdate?.Invoke(playerCreditsBalance);
    }

    public void SetCreditBalance(int amount)
    {
        playerCreditsBalance = amount;
    }

    public void AddCredits(int amount)
    {
        playerCreditsBalance += amount;
        OnCurrencyUpdate?.Invoke(playerCreditsBalance);
    }

    public void SubtractCredits(int amount)
    {
        playerCreditsBalance -= amount;
        OnCurrencyUpdate?.Invoke(playerCreditsBalance);
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

    // subscribe to enemy death event
    private void OnEnable()
    {
        BaseEnemy.OnEnemyDied += HandleEnemyDied;
        EnemyManager.OnFreezeAbility += SubtractCredits;
        TowerManager.OnDamageAbility += SubtractCredits;
        TowerManager.OnBoostAbility += SubtractCredits;
        EnemyWaveSystem.OnSetCreditBalance += SetCreditBalance;
    }
    // unsubscribe to enemy death event
    private void OnDisable()
    {
        BaseEnemy.OnEnemyDied -= HandleEnemyDied;
        EnemyManager.OnFreezeAbility -= SubtractCredits;
        TowerManager.OnDamageAbility -= SubtractCredits;
        TowerManager.OnBoostAbility -= SubtractCredits;
        EnemyWaveSystem.OnSetCreditBalance -= SetCreditBalance;
    }

    // event handler: runs when enemy dies
    private void HandleEnemyDied(GameObject enemy, int credits)
    {
        AddCredits(credits);
    }
}
