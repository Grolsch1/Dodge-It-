using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int damage = 10;
    public float moveSpeed = 5f;

    private PlayerHealth healthScript;
    private PlayerController movementScript;

void Start()
{
    healthScript = GetComponent<PlayerHealth>();
    movementScript = GetComponent<PlayerController>();
}

    public void IncreaseHealth(int amount)
    {
        maxHealth += amount;
        healthScript.maxHealth = maxHealth;
        healthScript.currentHealth += amount;
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        movementScript.IncreaseSpeed(moveSpeed);
    }
}