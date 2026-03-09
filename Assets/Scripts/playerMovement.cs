using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 targetPosition;
    private bool hasTarget = false;

    private Rigidbody2D playerBody2D;
    [SerializeField] private float speed;
    [SerializeField] private GameObject clickIndicatorPrefab;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDirection;

    void Start()
    {
        playerBody2D = GetComponent<Rigidbody2D>();
    }

  
    void Update()
    {
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = worldPos;
            hasTarget = true;
            Instantiate(clickIndicatorPrefab, worldPos, Quaternion.identity);
        }
        if (Input.GetMouseButtonDown(1) && dashCooldownTimer <=0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDirection = (mousePos - playerBody2D.position).normalized;

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }
    }

    private void Move()
    {
        if (!hasTarget)
            return;

        Vector2 direction = (targetPosition - playerBody2D.position).normalized;
        playerBody2D.linearVelocity = direction * speed;

        transform.up = direction;


        dashCooldownTimer -= Time.fixedDeltaTime;

        if (isDashing)
        {
            playerBody2D.linearVelocity = dashDirection * dashSpeed;

            dashTimer -= Time.fixedDeltaTime;

            if(dashTimer <= 0)
                isDashing = false;

            return;
        }

        if (hasTarget)
        {
            Vector2 direction = (targetPosition - playerBody2D.position).normalized;
            playerBody2D.linearVelocity = direction * speed;

            if (Vector2.Distance(playerBody2D.position, targetPosition) <0.1f)
            {
                playerBody2D.linearVelocity = Vector2.zero;
                hasTarget = false;
            }
        }
    }
}
