using UnityEngine;

public class Rocket : MonoBehaviour
{
    public ParticleSystem exlosion;

    private float speed;
    private GameObject target;
    private int damage;

    void Start()
    {
        exlosion.Stop();
    }

    public void SetTargetAndData(GameObject newTarget, float rocketSpeed, int damageAmount)
    {
        target = newTarget;
        speed = rocketSpeed;
        damage = damageAmount;
    }

    void Update()
    {
        if (target == null) return;
        Vector3 direction = target.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );
    }

    private void OnCollisionEnter(Collision col)
    {

        // enemy
        if (col.transform.parent.gameObject.tag == "Enemy")
        {
            // deal damage
            col.gameObject.GetComponentInParent<BaseEnemy>().TakeDamage(damage);

            // explosion
            exlosion.Play();
            // destory rocket
            Destroy(gameObject, 0.3f);
        }
    }
}
