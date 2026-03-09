using UnityEngine;
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
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        HandleDashCooldown();
        Move();
    }

    private void GetMouseInput()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //Movement
        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = mouseWorld;
            hasTarget = true;

            if (clickIndicatorPrefab != null)
                Instantiate(clickIndicatorPrefab, mouseWorld, Quaternion.identity);
        }


        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <=0)
        { 
            dashDirection = (mouseWorld - playerBody2D.position).normalized;

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            targetPosition = mouseWorld;
            hasTarget = true;
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

            if(dashTimer <= 0)
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
}
