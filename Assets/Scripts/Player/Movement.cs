using System;
using System.Collections;
using UnityEngine;
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
        [SerializeField] private float dashMinTime = 0.2f;
        [SerializeField] private float dashMaxTime = 1f;
        [SerializeField] private float dashIncreasePerSecond = 0.25f;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private float dashRechargeRate = 0.025f;
        [SerializeField] private float bounceCooldown;
        [SerializeField] private float bounceAbsorption;
        [SerializeField] private int maxNumberOfWallBounces = 2;

        public bool IsCurrentlyDashing { get; private set; }
        private bool IsBouncing { get; set; }
        private bool _chargingDash;

        private int _numberOfWallBounces;

        private Vector2 currentVelocity;
        private Rigidbody2D rb;
        private Vector2 moveInput;

        private float currentTimestamp = 0f;

        private PlayerAnimator playerAnimator;

        void Start()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float axisX = Input.GetAxisRaw("Horizontal");
            float axisY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(axisX, axisY).normalized;
            if (!IsCurrentlyDashing)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!IsCurrentlyDashing)
                    {
                        if (rb.velocity.x != 0 || rb.velocity.y != 0)
                        {
                            _chargingDash = true;
                            StartCoroutine(ChargeDash());
                        }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _chargingDash = false;
            }

            if (IsBouncing)
            {
                moveInput = Vector2.zero;
                return;
            }

            if (!Mathf.Approximately(currentTimestamp, Timestamp))
            {
                currentTimestamp = Timestamp;
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

        private IEnumerator ChargeDash()
        {
            IsCurrentlyDashing = true;
            rb.velocity = Vector2.zero;
            playerAnimator.PlayDash(currentVelocity);
            rb.velocity = currentVelocity * 0.25f;
            float power = 0;
            while (_chargingDash)
            {
                yield return new WaitForSeconds(0.05f);
                power = Mathf.Min(power + 0.025f, 1f);
            }

            StartCoroutine(Dash(Mathf.Max(0.1f, power)));
        }

        private IEnumerator Dash(float power)
        {
            Vector2 velocityVector;
            if (Mathf.Abs(moveInput.x) > 0 || Math.Abs(moveInput.y) > 0)
            {
                velocityVector = moveInput * (dashSpeed * maxSpeed * power * 2f);
            }
            else
            {
                velocityVector = currentVelocity * (dashSpeed * maxSpeed * power);
            }

            currentVelocity = Vector2.ClampMagnitude(velocityVector, dashSpeed);
            rb.velocity = Vector2.zero;
            rb.velocity = currentVelocity;
            playerAnimator.PlayDash(currentVelocity);
            yield return new WaitForSeconds(power);
            IsCurrentlyDashing = false;
            IsDashing?.Invoke(false);
            yield return new WaitForSeconds(dashCoolDown);
        }

        public void ChangeVelocity(float multiplier)
        {
            maxSpeed *= multiplier;
        }

        public void AttemptBounce(Vector2 normal)
        {
            if (IsCurrentlyDashing)
            {
                if (IsBouncing || _numberOfWallBounces == 0)
                {
                    return;
                }

                StartCoroutine(BounceRoutine());

                if (!(normal.x * currentVelocity.x >= 0 && normal.y * currentVelocity.y >= 0))
                {
                    currentVelocity = Vector2.Reflect(currentVelocity, normal);
                    currentVelocity = Vector2.ClampMagnitude(currentVelocity, dashSpeed);
                    rb.velocity = currentVelocity;
                    playerAnimator.PlayDash(currentVelocity);
                }

                if (_numberOfWallBounces > 0)
                {
                    _numberOfWallBounces--;
                }

                return;
            }

            if (Mathf.Approximately(currentVelocity.magnitude, 0))
            {
                currentVelocity = Vector2.zero;
            }

            if (!(normal.x * currentVelocity.x >= 0 && normal.y * currentVelocity.y >= 0))
            {
                StartCoroutine(BounceRoutine());

                currentVelocity = Vector2.Reflect(currentVelocity, normal);
                currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed) * (1 - bounceAbsorption);
                rb.velocity = currentVelocity;
            }
        }

        private IEnumerator BounceRoutine()
        {
            IsBouncing = true;
            yield return new WaitForSeconds(bounceCooldown);
            IsBouncing = false;
        }
    }
}