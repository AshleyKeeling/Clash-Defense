using UnityEngine;

public class PlayerBaseHealth : MonoBehaviour
{
    // events
    public static event System.Action<float, float> OnUpdateHealth;
    public static event System.Action OnPlayerBaseDestroyed;

    // variables
    public float maxHealth = 100f;
    public float health;

    void Start()
    {
        health = maxHealth;
        // update health bar
        OnUpdateHealth?.Invoke(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        // damage to health
        health -= damage;

        // update health bar
        OnUpdateHealth?.Invoke(health, maxHealth);

        if (health <= 0)
        {
            OnPlayerBaseDestroyed?.Invoke();
        }
    }

}
