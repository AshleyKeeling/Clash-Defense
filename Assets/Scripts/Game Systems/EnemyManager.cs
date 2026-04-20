using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public float abilityDuration = 10f;
    public int FreezeAbilityCost = 50;
    private UIManager uIManager;
    private CurrencyManager currencyManager;


    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        currencyManager = FindObjectOfType<CurrencyManager>();
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public virtual void ExecuteFreezeAbility()
    {
        StartCoroutine("FreezeAbility");
    }

    protected virtual IEnumerator FreezeAbility()
    {
        // check if player can afford first
        if (currencyManager.CanPlayerAfford(FreezeAbilityCost))
        {
            // subtract currency
            currencyManager.SubtractCredits(FreezeAbilityCost);

            // disable btn to avoid being double clicked
            uIManager.SetFreezeButtonState(false);

            // enable isFreezeAbilityEnabled to true
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<BaseEnemy>().isFreezeAbilityEnabled = true;
            }

            // wait x amount of time
            yield return new WaitForSeconds(abilityDuration);

            // disable isFreezeAbilityEnable to false
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<BaseEnemy>().isFreezeAbilityEnabled = false;
            }

            // re enables btn
            uIManager.SetFreezeButtonState(true);
        }
    }
}
