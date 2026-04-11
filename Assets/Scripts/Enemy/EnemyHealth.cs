using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int xpReward = 20;
    [SerializeField] private EnemyType enemyType;
    private bool isDead = false;
    public enum EnemyType
    {
        Normal,
        MiniBoss,
        Boss
    }

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        AudioManager.instance.PlaySFX("CatDeath");

        PlayerXP.instance.AddXP(xpReward);
        GameManager.instance.AddKill();

        if (enemyType == EnemyType.Boss)
        {
            GameManager.instance.OnBossKilled();
        }

        if (EnemySpawner.instance != null)
        {
            EnemySpawner.instance.OnEnemyKilled();
        }

        Destroy(gameObject);
    }
}