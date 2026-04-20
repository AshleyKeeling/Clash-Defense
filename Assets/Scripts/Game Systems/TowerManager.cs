using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public AbilitySciptableObject DamageAbilityData;
    public AbilitySciptableObject BoostAbilityData;
    public List<GameObject> towers;
    private UIManager uIManager;
    private CurrencyManager currencyManager;

    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        currencyManager = FindObjectOfType<CurrencyManager>();
    }

    // used to add to list every time a new tower is placed by player
    public void AddTower(GameObject newTower)
    {
        towers.Add(newTower);
    }

    public virtual void ExecuteDamageAbility()
    {
        StartCoroutine("DamageAbility");
    }

    public virtual void ExecuteBoostAbility()
    {
        StartCoroutine("BoostAbility");
    }

    // increases tower damage for x amount of time
    protected virtual IEnumerator DamageAbility()
    {
        // check if player can afford first
        if (currencyManager.CanPlayerAfford(DamageAbilityData.cost))
        {
            // subtract currency
            currencyManager.SubtractCredits(DamageAbilityData.cost);

            // disable btn to avoid being double clicked
            uIManager.SetDamageButtonState(false);

            // enable isDamageAbilityEnabled to true
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<BaseTowerController>().towerData.isDamageAbilityEnabled = true;
            }

            // wait x amount of time
            yield return new WaitForSeconds(DamageAbilityData.duration);

            // disable isDamageAbilityEnable to false
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<BaseTowerController>().towerData.isDamageAbilityEnabled = false;
            }

            // re enables btn
            uIManager.SetDamageButtonState(true);
        }
    }

    // increases fire speed for x amaount of time
    protected virtual IEnumerator BoostAbility()
    {
        // check if player can afford first
        if (currencyManager.CanPlayerAfford(BoostAbilityData.cost))
        {
            // subtract currency
            currencyManager.SubtractCredits(BoostAbilityData.cost);

            // disable btn to avoid being double clicked
            uIManager.SetBoostButtonState(false);

            // enable isDamageAbilityEnabled to true
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<BaseTowerController>().towerData.isBoostAbilityEnabled = true;
            }

            // wait x amount of time
            yield return new WaitForSeconds(BoostAbilityData.duration);

            // disable isDamageAbilityEnable to false
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<BaseTowerController>().towerData.isBoostAbilityEnabled = false;
            }

            // re enables btn
            uIManager.SetBoostButtonState(true);
        }
    }
}
