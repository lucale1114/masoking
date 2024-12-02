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

        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 75f;
        [SerializeField] private float deceleration = 25f;
        [SerializeField] private float turnDeceleration = 75f;
        [SerializeField] private float dashSpeed = 5f;
        [SerializeField] private float dashTime = 0.2f;
        [SerializeField] private float dashWindupTime = 0.25f;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private float dashRechargeRate = 0.025f;
        [SerializeField] private float bounceCooldown;
        [SerializeField] private float bounceAbsorption;
        [SerializeField] private int maxNumberOfWallBounces = 2;

        public bool IsCurrentlyDashing { get; private set; }
        private bool IsBouncing { get; set; }

        private int _numberOfWallBounces;

        private Vector2 currentVelocity;
        private Rigidbody2D rb;
        private Vector2 moveInput;

        private float currentTimestamp = 0f;
        private float dashPower = 3.0f;
        private bool dashFest;
        private Slider dashFill1;
        private Slider dashFill2;
        private Slider dashFill3;

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
            if (IsCurrentlyDashing)
            {
                return;
            }

            // Capture input
            float axisX = Input.GetAxisRaw("Horizontal");
            float axisY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(axisX, axisY).normalized;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!IsCurrentlyDashing && (dashPower >= 1 || dashFest))
                {
                    if (rb.velocity.x != 0 || rb.velocity.y != 0)
                    {
                        if (!dashFest)
                        {
                            dashPower -= 1f;
                        }

                        UpdateBars();
                        StartCoroutine(Dash());
                    }
                }
            }

            if (IsBouncing)
            {
                moveInput = Vector2.zero;
                return;
            }

            if (!Mathf.Approximately(currentTimestamp, Timestamp))
            {
                currentTimestamp = Timestamp;
                dashPower = Mathf.Min(dashPower + dashRechargeRate, 3);
                UpdateBars();
            }

            if (!IsCurrentlyDashing)
            {
                if (Mathf.Approximately(currentVelocity.magnitude, 0))
                {
                    playerAnimator.PlayIdle();
                }
                else
                {
                    playerAnimator.PlayMoving(currentVelocity);
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
            if (IsCurrentlyDashing || IsBouncing)
            {
                moveInput = Vector2.zero;
                return;
            }

            _numberOfWallBounces = maxNumberOfWallBounces;

            var targetSpeedX = moveInput.x != 0 ? maxSpeed : 0;
            var targetSpeedY = moveInput.y != 0 ? maxSpeed : 0;

            if (Mathf.Approximately(moveInput.x, 0))
            {
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else if (moveInput.x * currentVelocity.x < 0)
            {
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, turnDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetSpeedX * moveInput.x,
                    acceleration * Time.fixedDeltaTime);
            }

            if (Mathf.Approximately(moveInput.y, 0))
            {
                currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0, deceleration * Time.fixedDeltaTime);
            }
            else if (moveInput.y * currentVelocity.y < 0)
            {
                currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0, turnDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, targetSpeedY * moveInput.y,
                    acceleration * Time.fixedDeltaTime);
            }

            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);

            rb.velocity = currentVelocity;
        }

        private IEnumerator Dash()
        {
            IsCurrentlyDashing = true;
            IsDashing?.Invoke(true);
            currentVelocity = Vector2.ClampMagnitude(dashSpeed * maxSpeed * currentVelocity, dashSpeed);
            rb.velocity = Vector2.zero;
            playerAnimator.PlayDash(currentVelocity);
            yield return new WaitForSeconds(dashWindupTime);
            rb.velocity = currentVelocity;
            yield return new WaitForSeconds(dashTime);
            IsCurrentlyDashing = false;
            IsDashing?.Invoke(false);
            yield return new WaitForSeconds(dashCoolDown);
        }

        public void DashFest(bool isDashFest)
        {
            dashFest = isDashFest;
        }

        public void ChangeVelocity(float multiplier)
        {
            maxSpeed *= multiplier;
        }

        public void AttemptBounce(Vector2 normal)
        {
            if (IsCurrentlyDashing)
            {
                if (_numberOfWallBounces == 0)
                {
                    if (!(normal.x * currentVelocity.x >= 0 && normal.y * currentVelocity.y >= 0))
                    {
                        StartCoroutine(BounceRoutine());
                        currentVelocity = Vector2.Reflect(currentVelocity, normal);
                        currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
                        rb.velocity = currentVelocity;
                    }

                    return;
                }
            }

            if (Mathf.Approximately(currentVelocity.magnitude, 0))
            {
                currentVelocity = Vector2.zero;
            }

            if (!(normal.x * currentVelocity.x >= 0 && normal.y * currentVelocity.y >= 0))
            {
                StartCoroutine(BounceRoutine());
                if (IsCurrentlyDashing)
                {
                    currentVelocity = Vector2.Reflect(currentVelocity, normal);
                    _numberOfWallBounces--;
                }
                else
                {
                    currentVelocity = Vector2.Reflect(currentVelocity, normal) * (1 - bounceAbsorption);
                }
            }

            rb.velocity = currentVelocity;
        }

        private IEnumerator BounceRoutine()
        {
            IsBouncing = true;
            yield return new WaitForSeconds(bounceCooldown);
            IsBouncing = false;
        }
    }
}