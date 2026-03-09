using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private int damage = 10;
    private float lifetime = 2f;
    private float timer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if( timer <= 0 )
        {
            ReturnToPool();
        }
    }
    public void Initialize(Vector2 direction, float speed, int damageAmount)
    {
        damage = damageAmount;
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TurretEnemy enemy = collision.GetComponent<TurretEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            ReturnToPool();
        }   
    }

    void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        PlayerBulletPool.Instance.ReturnBullet(gameObject);
    }
}
