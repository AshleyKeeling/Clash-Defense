using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    // events
    public static event System.Action<int> OnDamageAbility;
    public static event System.Action<int> OnBoostAbility;
    public static event System.Action<bool> OnDamageAbilityBtn;
    public static event System.Action<bool> OnBoostAbilityBtn;
    // variables
    public AbilitySciptableObject DamageAbilityData;
    public AbilitySciptableObject BoostAbilityData;
    public List<GameObject> towers;
    private CurrencyManager currencyManager;

    private void Start()
    {
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
            OnDamageAbility?.Invoke(DamageAbilityData.cost);


            // disable btn to avoid being double clicked
            OnDamageAbilityBtn?.Invoke(false);

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
            OnDamageAbilityBtn?.Invoke(true);
        }
    }

    // increases fire speed for x amaount of time
    protected virtual IEnumerator BoostAbility()
    {
        // check if player can afford first
        if (currencyManager.CanPlayerAfford(BoostAbilityData.cost))
        {
            // subtract currency
            OnBoostAbility?.Invoke(BoostAbilityData.cost);

            // disable btn to avoid being double clicked
            OnBoostAbilityBtn?.Invoke(false);

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
            OnBoostAbilityBtn?.Invoke(true);
        }
    }

    private void DestroyAllTowers()
    {
        // destroy all towers
        foreach (GameObject tower in towers)
        {
            Destroy(tower);
        }
        // resets list
        towers.Clear();
    }


    private void OnEnable()
    {
        TowerDragUI.OnTowerCreated += AddTower;
        EnemyWaveSystem.OnLevelEnded += HandleDestroyAllTowers;
    }

    private void OnDisable()
    {
        TowerDragUI.OnTowerCreated -= AddTower;
        EnemyWaveSystem.OnLevelEnded -= HandleDestroyAllTowers;
    }

    private void HandleDestroyAllTowers(LevelEndData data)
    {
        DestroyAllTowers();
    }
}
