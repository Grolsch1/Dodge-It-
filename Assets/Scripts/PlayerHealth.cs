using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private DamageUI damageUI;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (damageUI != null)
            damageUI.TriggerFlash();

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
        
    }

    void Die()
    {
        Debug.Log("Player Died");
        Destroy(gameObject);
    }
}
