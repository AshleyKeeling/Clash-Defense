using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameThrowerTowerController : BaseTowerController
{
    public float flameEffectRaduis = 1f;

    private List<GameObject> GetAllEnimiesWithInFlameEffect(Transform lockedEnemy)
    {
        List<GameObject> enemiesWithInRange = new List<GameObject>();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(lockedEnemy.position, enemy.transform.position);

            if (distance <= flameEffectRaduis)
            {
                enemiesWithInRange.Add(enemy);
            }
        }

        return enemiesWithInRange;
    }

    protected IEnumerator Fire(List<GameObject> targets)
    {
        canFire = false;

        shootEffect.Play();
        PlayShootSFX(true);

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            BaseEnemy enemy = target.GetComponent<BaseEnemy>();
            if (enemy == null) continue;

            if (towerData.isDamageAbilityEnabled)
            {
                enemy.TakeDamage(towerData.abilityBulletDamage);
            }
            else
            {
                enemy.TakeDamage(towerData.bulletDamage);
            }
        }

        if (towerData.isBoostAbilityEnabled)
        {
            yield return new WaitForSeconds(towerData.abilityFireRate);
        }
        else
        {
            yield return new WaitForSeconds(towerData.fireRate);
        }

        shootEffect.Stop();
        PlayShootSFX(false);
        canFire = true;
    }

    protected override void Shoot(GameObject target)
    {

        Vector3 dir = target.transform.position - gunObj.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        gunObj.rotation = Quaternion.Slerp(gunObj.rotation, rot, 10f * Time.deltaTime);

        List<GameObject> enemiesWithInRange = GetAllEnimiesWithInFlameEffect(target.transform);
        if (canFire)
        {
            StartCoroutine(Fire(enemiesWithInRange));
        }
    }
}