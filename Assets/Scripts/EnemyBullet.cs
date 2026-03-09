using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;
    private float timer;
    private Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        direction = direction.normalized;
        rb.linearVelocity = direction * speed;
        timer = lifetime;
    }

    void Update ()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            BulletPool.Instance.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player != null)
                player.TakeDamage(damage);

            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}