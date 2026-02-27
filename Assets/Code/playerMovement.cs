using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 targetPosition;
    private bool hasTarget = false;

    private Rigidbody2D playerBody2D;
    [SerializeField] private float speed;

    void Start()
    {
        playerBody2D = GetComponent<Rigidbody2D>();
    }

  
    void Update()
    {
        LookAtMouse();
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hasTarget = true;
        }
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hasTarget = true;
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
