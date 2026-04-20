using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FlameThrowerTowerController : BaseTowerController
{
    public float flameEffectRaduis = 1f;

    //returns all enemies with in the flame effect raduis of the locked target
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

        // return list
        return enemiesWithInRange;
    }

    protected override void Shoot(GameObject target)
    {
        // aim at enemy
        Vector3 dir = target.transform.position - gunObj.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        gunObj.rotation = Quaternion.Slerp(gunObj.rotation, rot, 10f * Time.deltaTime);

        // find all enemies with flame effect raduis(splash damage)
        List<GameObject> enmiesWithInRange = GetAllEnimiesWithInFlameEffect(target.transform);

        // do damage to all of them
        foreach (GameObject enemyInFlameEffect in enmiesWithInRange)
        {
            Fire(enemyInFlameEffect);
        }

        // fire gun
        StartCoroutine(Fire(target));
    }
}
