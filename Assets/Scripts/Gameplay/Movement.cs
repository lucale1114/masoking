using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private float currentSpeed = 0f;
    // Dash Variables
    private bool canDash = true;
    private bool isDashing;
    private float dashTime = 0.2f;
    private float dashCoolDown;

    [SerializeField]
    private float dashSpeed = 5f;

    void Start()
    {
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
        if (moveInput.magnitude > 0)
        {
            currentVelocity = moveInput;
        }

        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        // If there's input, accelerate
        if (moveInput.magnitude > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
        }
        else // If no input, decelerate
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }

        // Set the velocity of the Rigidbody
        rb.velocity = currentVelocity * currentSpeed;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity =  currentSpeed * currentVelocity * dashSpeed;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
