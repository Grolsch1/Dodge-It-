using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int xpReward = 20;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        AudioManager.instance.PlaySFX("EnemyDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.instance.PlaySFX("CatDeath");

        PlayerXP.instance.AddXP(xpReward);
        GameManager.instance.AddKill();

        Destroy(gameObject);
    }
}