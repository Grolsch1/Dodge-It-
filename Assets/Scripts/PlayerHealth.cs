using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int maxHealth = 100;
    public int currentHealth;

    [Header("Effects")]
    [SerializeField] private DamageUI damageUI;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (damageUI != null)
            damageUI.TriggerFlash();

        //Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
        
    }

    void Die()
    {
        GameManager.instance.PlayerDied();
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

}
