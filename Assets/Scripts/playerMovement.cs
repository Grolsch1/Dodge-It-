using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;

    private Vector2 targetPosition;
    private bool hasTarget;

    [Header("Movement")]
    [SerializeField] public float speed = 5f;

    [Header("Click Indicator")]
    [SerializeField] private GameObject clickIndicatorPrefab;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDirection;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] public int damage = 10;
    [SerializeField] private float fireRate = 0.25f;

    private float shootCooldown;
    private bool isShooting;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!GameManager.instance.isGamePlaying)
            return;

        HandleInput();
    }

    void FixedUpdate()
    {
        HandleDashCooldown();
        Move();
    }

    void HandleInput()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        shootCooldown -= Time.deltaTime;

        // SHOOT
        if (Input.GetKeyDown(KeyCode.C) && shootCooldown <= 0)
        {
            Shoot(mouseWorld);
        }

        // Stop movement while shooting
        if (isShooting)
            return;

        // MOVEMENT (Right Click)
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            targetPosition = mouseWorld;
            hasTarget = true;

            if (clickIndicatorPrefab != null)
                Instantiate(clickIndicatorPrefab, mouseWorld, Quaternion.identity);
        }

        // DASH
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            dashDirection = (mouseWorld - rb.position).normalized;

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            targetPosition = mouseWorld;
            hasTarget = true;
        }
    }

    void Shoot(Vector2 target)
    {
        if (firePoint == null) return;

        isShooting = true;

        GameObject proj = PlayerBulletPool.Instance.GetBullet();

        proj.transform.position = firePoint.position;

        Vector2 direction = (target - (Vector2)firePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);

        PlayerProjectile projectileScript = proj.GetComponent<PlayerProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, projectileSpeed, damage);
        }

        shootCooldown = fireRate;
        isShooting = false;
    }

    void HandleDashCooldown()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.fixedDeltaTime;
    }

    void Move()
    {
        // DASH MOVEMENT
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;

            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0)
                isDashing = false;

            return;
        }

        // NORMAL MOVEMENT
        if (!hasTarget)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.linearVelocity = direction * speed;

        float distance = Vector2.Distance(rb.position, targetPosition);

        if (distance <= speed * Time.fixedDeltaTime)
        {
            rb.position = targetPosition;
            hasTarget = false;
        }
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }
}
