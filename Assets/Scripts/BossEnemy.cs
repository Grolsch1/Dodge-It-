using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public int maxHealth = 200;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optional: add explosion / effects here

        GameManager.instance.OnBossKilled();

        Destroy(gameObject);
    }
}
