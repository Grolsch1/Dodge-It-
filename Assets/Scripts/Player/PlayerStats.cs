using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;
    public int damage = 10;

    private PlayerHealth health;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    public void IncreaseHealth(int amount)
    {
        health.IncreaseMaxHealth(amount);
    }
}