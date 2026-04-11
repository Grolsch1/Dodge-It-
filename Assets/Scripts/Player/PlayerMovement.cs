using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    private PlayerStats stats;

    private Vector2 targetPosition;
    private Vector2 movementInput;
    private bool hasTarget;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDirection;

    [Header("Click Indicator")]
    [SerializeField] private GameObject clickIndicatorPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        stats = GetComponent<PlayerStats>();
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

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        // Movement
        if (movementInput == Vector2.zero)
        {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                targetPosition = mouseWorld;
                hasTarget = true;

                if (clickIndicatorPrefab != null)
                    Instantiate(clickIndicatorPrefab, mouseWorld, Quaternion.identity);
            }
        }
        else
        {
            hasTarget = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            if (movementInput != Vector2.zero)
                dashDirection = movementInput;
            else
                dashDirection = (mouseWorld - rb.position).normalized;

            AudioManager.instance.PlaySFX("Dash");
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    void Move()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0)
                isDashing = false;

            return;
        }

            // WASD Movement
            if (movementInput != Vector2.zero)
            {
                rb.linearVelocity = movementInput * stats.moveSpeed;
                return;
            }

            // Click-to-move
            if (!hasTarget)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.linearVelocity = direction * stats.moveSpeed;

            float distance = Vector2.Distance(rb.position, targetPosition);

            if (distance <= stats.moveSpeed * Time.fixedDeltaTime)
            {
                rb.position = targetPosition;
                hasTarget = false;
            }
        }

    void HandleDashCooldown()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.fixedDeltaTime;
    }
}