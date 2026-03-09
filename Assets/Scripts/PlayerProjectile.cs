using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private int damage = 10;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, int damageAmount)
    {
        damage = damageAmount;
        rb.linearVelocity = direction.normalized * speed; // Set velocity
        Destroy(gameObject, 2f);              // Auto destroy
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TurretEnemy enemy = collision.GetComponent<TurretEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }   
    }
}
