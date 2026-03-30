using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int xpReward = 20;
    [SerializeField] private EnemyType enemyType;
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

        if (enemyType == EnemyType.Boss)
        {
            GameManager.instance.OnBossKilled();
        }
        Destroy(gameObject);
    }
}