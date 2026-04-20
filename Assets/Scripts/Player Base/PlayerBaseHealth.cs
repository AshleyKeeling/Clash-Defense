using UnityEngine;

public class PlayerBaseHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    private UIManager uIManager;
    private GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        uIManager = FindObjectOfType<UIManager>();
        uIManager.UpdatePlayerBaseHealthBar(health, maxHealth);
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(float damage)
    {
        // damage to health
        health -= damage;

        // update health bar
        uIManager.UpdatePlayerBaseHealthBar(health, maxHealth);

        if (health <= 0)
        {
            gameManager.GameOver();
        }
    }

}
