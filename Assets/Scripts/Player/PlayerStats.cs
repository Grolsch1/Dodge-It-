using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;
    public int damage = 10;

    [SerializeField] private PlayerHUD playerHUD;
    private PlayerHealth health;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        playerHUD.UpdateStats(this);
    }
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        playerHUD.UpdateStats(this);
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
        playerHUD.UpdateStats(this);
    }

    public void IncreaseHealth(int amount)
    {
        health.IncreaseMaxHealth(amount);
    }
}