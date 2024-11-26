using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static WaveData;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public Action<bool> IsDashing;
        public bool IsCurentlyDashing => isDashing;
        public float maxSpeed = 5f;
        public float acceleration = 75f;
        public float deceleration = 75f;
        public Vector2 currentVelocity;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private readonly float currentSpeed = 0f;
        // Dash Variables
        private bool canDash = true;
        public bool isDashing;
        private readonly float dashTime = 0.2f;
        private readonly float dashCoolDown = 0;
        private float currentTimestamp = 0f;
        public float dashPower = 3.0f;
        private Slider dashFill1;
        private Slider dashFill2;
        private Slider dashFill3;

        [SerializeField] private float dashSpeed = 5f;

        private PlayerAnimator playerAnimator;
        void Start()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
            rb = GetComponent<Rigidbody2D>();
            dashFill1 = GameObject.Find("Fill1").transform.parent.GetComponent<Slider>();
            dashFill2 = GameObject.Find("Fill2").transform.parent.GetComponent<Slider>();
            dashFill3 = GameObject.Find("Fill3").transform.parent.GetComponent<Slider>();
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
            if (Input.GetKeyDown(KeyCode.Space) && canDash && dashPower >= 1)
            {
                if (rb.velocity.x != 0 || rb.velocity.y != 0)
                {
                    dashPower -= 1f;
                    UpdateBars();
                    StartCoroutine(Dash());
                }
            }
            if (currentTimestamp != Timestamp)
            {
                currentTimestamp = Timestamp;
                dashPower = Mathf.Min(dashPower + 0.025f, 3);
                UpdateBars();
            }
            if (!isDashing)
            {
                if (Mathf.Approximately(rb.velocity.magnitude, 0))
                {
                    playerAnimator.PlayIdle();
                }
                else
                {
                    playerAnimator.PlayMoving(moveInput.x, moveInput.y);
                }
            }
        }

        void UpdateBars()
        {
            dashFill1.value = dashPower - 2;
            dashFill2.value = dashPower - 1;
            dashFill3.value = dashPower;
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

        public IEnumerator Dash()
        {
            canDash = false;
            isDashing = true;
            IsDashing?.Invoke(true);
            rb.velocity =  dashSpeed * currentVelocity;
            playerAnimator.PlayDash(moveInput.x, moveInput.y);
            yield return new WaitForSeconds(dashTime);
            isDashing = false;
            IsDashing?.Invoke(false);
            yield return new WaitForSeconds(dashCoolDown);
            canDash = true;
        }


    }
}
