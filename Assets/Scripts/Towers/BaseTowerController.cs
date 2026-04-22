using UnityEngine;
using System.Collections;

public class BaseTowerController : MonoBehaviour
{
    public Transform gunObj;
    public ParticleSystem shootEffect;
    public TowerDataScriptableObject towerData;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // towerData = GetComponent<TowerData>();
        shootEffect.Stop();
    }

    void Update()
    {
        // stops the tower from firing if the game is paused
        if (Time.timeScale == 0f) return;
        LookForEnimies();
    }

    private void LookForEnimies()
    {
        // wait till enemy appears in range
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= towerData.viewRadius)
            {
                Shoot(enemy);
                return; // stop after finding first enemy
            }
        }
    }

    protected virtual void Shoot(GameObject target)
    {
        // aim at enemy
        Vector3 dir = target.transform.position - gunObj.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        gunObj.rotation = Quaternion.Slerp(gunObj.rotation, rot, 10f * Time.deltaTime);

        // fire gun
        if (towerData.canFire)
        {
            StartCoroutine(Fire(target));
        }
    }

    protected virtual IEnumerator Fire(GameObject target)
    {
        towerData.canFire = false;

        // damage to enemey, bullet damage depends on if player has chose the damage ability
        if (towerData.isDamageAbilityEnabled)
        {
            target.GetComponent<BaseEnemy>().TakeDamage(towerData.abilityBulletDamage);
        }
        else
        {
            target.GetComponent<BaseEnemy>().TakeDamage(towerData.bulletDamage);
        }

        shootEffect.Play();

        // controlls the speed of the guns fire rate, fire rate depends if the player chose the boost ability
        if (towerData.isBoostAbilityEnabled)
        {
            yield return new WaitForSeconds(towerData.abilityFireRate);
        }
        else
        {
            yield return new WaitForSeconds(towerData.fireRate);
        }

        shootEffect.Stop();
        towerData.canFire = true;
    }

    public void PlayTowerPlacementSFX()
    {
        audioSource.PlayOneShot(towerData.towerPlacementSFX);
    }

    // -----------------------------------------------
    // Gizmos for debugging vision
    // -----------------------------------------------
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        int segments = 40;
        float angleStep = 360f / segments;

        Vector3 previousPoint = transform.position + new Vector3(towerData.viewRadius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = transform.position + new Vector3(
                Mathf.Cos(angle) * towerData.viewRadius,
                0,
                Mathf.Sin(angle) * towerData.viewRadius
            );

            Gizmos.DrawLine(previousPoint, newPoint);
            previousPoint = newPoint;
        }
    }
}
