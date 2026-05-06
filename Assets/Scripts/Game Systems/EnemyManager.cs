using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    // events
    public static event System.Action<int> OnFreezeAbility;
    public static event System.Action<bool> OnFreezeAbilityBtn;
    public static event System.Action OnAllEnemiesCleared;
    // variables
    public List<GameObject> enemies;
    public AbilitySciptableObject FreezeAbilityData;
    private CurrencyManager currencyManager;


    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            OnAllEnemiesCleared?.Invoke();
        }
    }

    public virtual void ExecuteFreezeAbility()
    {
        StartCoroutine("FreezeAbility");
    }

    protected virtual IEnumerator FreezeAbility()
    {
        // check if player can afford first
        if (currencyManager.CanPlayerAfford(FreezeAbilityData.cost))
        {
            // subtract currency
            OnFreezeAbility?.Invoke(FreezeAbilityData.cost);
            // currencyManager.SubtractCredits(FreezeAbilityData.cost);

            // disable btn to avoid being double clicked
            OnFreezeAbilityBtn?.Invoke(false);

            // enable isFreezeAbilityEnabled to true
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<BaseEnemy>().isFreezeAbilityEnabled = true;
            }

            // wait x amount of time
            yield return new WaitForSeconds(FreezeAbilityData.duration);

            // disable isFreezeAbilityEnable to false
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<BaseEnemy>().isFreezeAbilityEnabled = false;
            }

            // re enables btn
            OnFreezeAbilityBtn?.Invoke(true);
        }
    }

    // subscribe events
    private void OnEnable()
    {
        BaseEnemy.OnEnemyDied += HandleEnemyDied;
        EnemyWaveSystem.OnNewEnemy += AddEnemy;
    }
    // unsubscribe events
    private void OnDisable()
    {
        BaseEnemy.OnEnemyDied -= HandleEnemyDied;
        EnemyWaveSystem.OnNewEnemy -= AddEnemy;
    }
    // event handler: runs when enemy dies

    private void HandleEnemyDied(GameObject enemy, int credits)
    {
        RemoveEnemy(enemy);
    }
}
