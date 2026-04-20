using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{

    public float abilityDuration = 10f;
    public int DamageAbilityCost = 50;
    public int BoostAbilityCost = 60;
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
        if (currencyManager.CanPlayerAfford(DamageAbilityCost))
        {
            // subtract currency
            currencyManager.SubtractCredits(DamageAbilityCost);

            // disable btn to avoid being double clicked
            uIManager.SetDamageButtonState(false);

            // enable isDamageAbilityEnabled to true
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<BaseTowerController>().towerData.isDamageAbilityEnabled = true;
            }

            // wait x amount of time
            yield return new WaitForSeconds(abilityDuration);

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
        if (currencyManager.CanPlayerAfford(BoostAbilityCost))
        {
            // subtract currency
            currencyManager.SubtractCredits(BoostAbilityCost);

            // disable btn to avoid being double clicked
            uIManager.SetBoostButtonState(false);

            // enable isDamageAbilityEnabled to true
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<BaseTowerController>().towerData.isBoostAbilityEnabled = true;
            }

            // wait x amount of time
            yield return new WaitForSeconds(abilityDuration);

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
