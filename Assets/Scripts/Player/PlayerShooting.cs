using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float fireRate = 0.25f;

    [Header("Spread Shot Ability")]
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private KeyCode spreadKey = KeyCode.Space;
    [SerializeField] private float abilityCooldown = 2f;

    private float shootCooldown;
    private float abilityCooldownTimer;

    private Camera mainCamera;
    private PlayerStats stats;
    private PlayerMovement movement;

    void Awake()
    {
        mainCamera = Camera.main;
        stats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!GameManager.instance.isGamePlaying)
            return;

        shootCooldown -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;

        // Normal
        if (Input.GetMouseButtonDown(0) && shootCooldown <= 0f)
        {
            ShootSingle();
        }

        // Spread
        if (Input.GetKeyDown(spreadKey) && abilityCooldownTimer <= 0f)
        {
            ShootSpread();
            abilityCooldownTimer = abilityCooldown;
        }
    }

    void ShootSingle()
    {
        if (firePoint == null) return;

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - (Vector2)firePoint.position).normalized;

        GameObject proj = PlayerBulletPool.Instance.GetBullet();
        proj.transform.position = firePoint.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);

        PlayerProjectile projectileScript = proj.GetComponent<PlayerProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, projectileSpeed, stats.damage);
        }

        if (movement != null)
        {
            movement.ApplySlow(0.8f, 0.3f);
        }

        AudioManager.instance.PlaySFX("Shoot");
        shootCooldown = fireRate;
    }

    void ShootSpread()
    {
        if (firePoint == null) return;

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 baseDirection = (mouseWorld - (Vector2)firePoint.position).normalized;

        float halfSpread = spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float t = (bulletCount == 1) ? 0.5f : (float)i / (bulletCount - 1);
            float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, t);

            Vector2 newDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            GameObject proj = PlayerBulletPool.Instance.GetBullet();
            proj.transform.position = firePoint.position;

            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.Euler(0, 0, angle);

            PlayerProjectile projectileScript = proj.GetComponent<PlayerProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(newDirection.normalized, projectileSpeed, stats.damage);
            }
        }
        if (movement != null)
        {
            movement.ApplySlow(0.3f, 0.5f);
        }
        AudioManager.instance.PlaySFX("Shoot");
    }
}