using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int maxHealth = 100;
    [SerializeField] private PlayerHUD playerHUD;
    public int currentHealth;

    [Header("Effects")]
    [SerializeField] private DamageUI damageUI;

    void Awake()
    {
        currentHealth = maxHealth;
        playerHUD.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.instance.PlaySFX("TakeDamage");
        currentHealth -= damage;
        playerHUD.UpdateHealth(currentHealth, maxHealth);

        if (damageUI != null)
            damageUI.TriggerFlash();

        //Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
        
    }

    void Die()
    {
        AudioManager.instance.PlaySFX("Die");
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

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }

}
