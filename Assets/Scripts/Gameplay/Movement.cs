using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 4f;
    public Vector2 currentVelocity;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private readonly float currentSpeed = 0f;
    // Dash Variables
    private bool canDash = true;
    private bool isDashing;
    private readonly float dashTime = 0.2f;
    private readonly float dashCoolDown = 0;

    [SerializeField]
    private float dashSpeed = 5f;

    private PlayerAnimator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        // Capture input
        float axisX = Input.GetAxisRaw("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(axisX, axisY).normalized;

        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (!isDashing)
        {
            if (Mathf.Approximately(rb.velocity.magnitude, 0))
            {
                playerAnimator.SetIdle();
            }
            else
            {
                playerAnimator.SetMoving();
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // Set the Rigidbody's velocity
        rb.velocity = currentVelocity * currentSpeed;
        float targetSpeedX = moveInput.x != 0 ? maxSpeed : 0;
        float targetSpeedY = moveInput.y != 0 ? maxSpeed : 0;

        // Smoothly update current speed on each axis
        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetSpeedX * moveInput.x, acceleration * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, targetSpeedY * moveInput.y, acceleration * Time.fixedDeltaTime);

        // Apply deceleration only when no input is present on that axis
        if (moveInput.x == 0)
        {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }

        if (moveInput.y == 0)
        {
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0, deceleration * Time.fixedDeltaTime);
        }

        // Set the Rigidbody's velocity
        rb.velocity = currentVelocity;

    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity =  dashSpeed * currentVelocity;
        playerAnimator.TriggerDash();
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }


}
