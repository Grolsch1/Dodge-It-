using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float fireRate = 0.25f;

    private float shootCooldown;
    private Camera mainCamera;
    private PlayerStats stats;

    void Awake()
    {
        mainCamera = Camera.main;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!GameManager.instance.isGamePlaying)
            return;

        shootCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && shootCooldown <= 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoint == null) return;

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        GameObject proj = PlayerBulletPool.Instance.GetBullet();
        proj.transform.position = firePoint.position;

        Vector2 direction = (mouseWorld - (Vector2)firePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);

        PlayerProjectile projectileScript = proj.GetComponent<PlayerProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, projectileSpeed, stats.damage);
        }
        AudioManager.instance.PlaySFX("Shoot");

        shootCooldown = fireRate;
    }
}