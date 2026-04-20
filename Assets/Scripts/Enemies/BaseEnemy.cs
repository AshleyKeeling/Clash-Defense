using UnityEngine;
public class BaseEnemy : MonoBehaviour
{
    public float speed = 2f;
    private Transform[] pathPoints;
    private int currentPathPointIndex = 0;
    public float maxHealth = 100f;
    public float health;
    public float damageAmount = 10f;
    public int creditAmount = 0;
    public bool isFreezeAbilityEnabled = false;
    public GameObject playerBase;

    void Start()
    {
        // gets all points in path
        Transform pathParent = GameObject.FindGameObjectWithTag("Path").transform;

        pathPoints = new Transform[pathParent.childCount];
        for (int i = 0; i < pathParent.childCount; i++)
        {
            pathPoints[i] = pathParent.GetChild(i);
        }

        // sets enemy spawn to the first point
        transform.position = pathPoints[0].position;

        // sets health
        health = maxHealth;

        // gets player base
        playerBase = GameObject.FindGameObjectWithTag("PlayerBase");
    }

    void Update()
    {
        if (currentPathPointIndex == pathPoints.Length)
        {
            DamageToPlayerBase();
            return;
        }

        // get next point
        Transform target = pathPoints[currentPathPointIndex];

        // move enemy towards point
        if (!isFreezeAbilityEnabled)
        {
            transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
        }


        // rotation
        Vector3 dir = target.position - transform.position;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentPathPointIndex += 1;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // adds credits to player currency balance
            CurrencyManager currencyManager = FindObjectOfType<CurrencyManager>();
            currencyManager.AddCredits(creditAmount);

            // kills enemey
            Death();
            return;
        }
    }

    private void DamageToPlayerBase()
    {
        // damage player base
        playerBase.GetComponent<PlayerBaseHealth>().TakeDamage(damageAmount);
        // destory self
        Death();
    }

    public void Death()
    {
        // removes from list
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RemoveEnemy(gameObject);

        // destorys self
        Destroy(gameObject);
    }
}
