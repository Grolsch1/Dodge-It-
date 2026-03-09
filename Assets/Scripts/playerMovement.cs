using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D playerBody2D;
    private Camera mainCamera;

    private Vector2 targetPosition;
    private bool hasTarget;

    [SerializeField] private float speed = 5f;

    [Header("Click Indicator")]
    [SerializeField] private GameObject clickIndicatorPrefab;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // drag FirePoint here in Inspector
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int damage = 10;
    private bool isShooting = false;


    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDirection;

    private void Awake()
    {
        playerBody2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!GameManager.instance.isGamePlaying)
            return;

        GetMouseInput();
    }

    private void FixedUpdate()
    {
        if (!isShooting && GameManager.instance.isGamePlaying)
        {
            GetMouseInput();
        }

        // Shooting can happen regardless
        if (Input.GetKeyDown(KeyCode.C))
        {
            isShooting = true;
            Shoot(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            isShooting = false; // or delay if you want shooting animation/cast time
        }
        HandleDashCooldown();
        Move();
    }

    private void GetMouseInput()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //Movement and Click Indicator
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            targetPosition = mouseWorld;
            hasTarget = true;

            if (clickIndicatorPrefab != null)
                Instantiate(clickIndicatorPrefab, mouseWorld, Quaternion.identity);
        }


        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            dashDirection = (mouseWorld - playerBody2D.position).normalized;

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            targetPosition = mouseWorld;
            hasTarget = true;
        }
        // Shooting
        if (Input.GetKeyDown(KeyCode.C))
        {
            Shoot(mouseWorld);
        }
    }

    private void HandleDashCooldown()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.fixedDeltaTime;

    }

    private void Move()
    {
        //Dash Movement
        if (isDashing)
        {
            playerBody2D.linearVelocity = dashDirection * dashSpeed;

            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0)
                isDashing = false;

            return;
        }

        //Normal Movement
        if (!hasTarget)
        {
            playerBody2D.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (targetPosition - playerBody2D.position).normalized;
        playerBody2D.linearVelocity = direction * speed;

        float distance = Vector2.Distance(playerBody2D.position, targetPosition);

        if (distance <= speed * Time.fixedDeltaTime)
        {
            playerBody2D.position = targetPosition;
            hasTarget = false;
        }
    }

    private void Shoot(Vector2 target)
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Direction from fire point to cursor
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;

        // Spawn projectile at fire point
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Rotate projectile to face cursor (optional)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Initialize projectile
        PlayerProjectile projectileScript = proj.GetComponent<PlayerProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, projectileSpeed, damage);
        }
    }
}
