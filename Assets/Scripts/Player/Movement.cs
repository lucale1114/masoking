using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;

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
        [SerializeField] private float dashMinTime = 0.1f;
        [SerializeField] private float dashMaxTime = 0.5f;
        [SerializeField] private float dashIncrease = 0.025f;
        [SerializeField] private float bounceCooldown;
        [SerializeField] private float bounceAbsorption;
        [SerializeField] private int maxNumberOfWallBounces = 2;
        [SerializeField] private AudioClip walk;
        [SerializeField] private AudioClip[] dash;


        public bool IsCurrentlyDashing { get; private set; }
        public bool IsInDashState { get; private set; }
        private bool IsBouncing { get; set; }
        private bool _chargingDash;
        private float _dashCoolDown = 0.5f;
        private SpriteRenderer _dashImageCharger;
        private SpriteRenderer _plrSprite;

        private int _numberOfWallBounces;
        private float power;

        private Vector2 currentVelocity;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        public float knocked = 0;

        private float currentTimestamp = 0f;

        private PlayerAnimator playerAnimator;

        void Start()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
            rb = GetComponent<Rigidbody2D>();
            _plrSprite = GetComponent<SpriteRenderer>();
            _dashImageCharger = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _dashImageCharger.enabled = false;
        }

        void Update()
        {
            if (_dashImageCharger.enabled)
            {
                _dashImageCharger.sprite = _plrSprite.sprite;
            }
            float axisX = Input.GetAxisRaw("Horizontal");
            float axisY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(axisX, axisY).normalized;
            if (!IsCurrentlyDashing)
            {
                if (Input.GetAxis("Jump") == 1 && _dashCoolDown >= 0.5f)
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

            if (Input.GetAxis("Jump") == 0)
            {
                _chargingDash = false;
            }

            if (IsBouncing)
            {
                moveInput = Vector2.zero;
                return;
            }

            if (!Mathf.Approximately(currentTimestamp, WaveHandler.Timestamp))
            {
                if (!IsCurrentlyDashing)
                {
                    _dashCoolDown = Mathf.Min(_dashCoolDown + 0.1f, 1);
                }
                knocked = Mathf.Max(0, knocked - 0.1f);
                currentTimestamp = WaveHandler.Timestamp;
            }

            if (!IsCurrentlyDashing)
            {
                if (Mathf.Approximately(currentVelocity.magnitude, 0))
                {
                    playerAnimator.PlayIdle();
                    SoundFXManager.Instance.StopWalking();
                }
                else
                {
                    playerAnimator.PlayMoving(currentVelocity);
                    SoundFXManager.Instance.StartWalking();
                }
            }
        }



        void FixedUpdate()
        {
            if (IsInDashState || IsBouncing)
            {
                moveInput = Vector2.zero;
                return;
            }
            else if ((!IsInDashState && IsCurrentlyDashing))
            {
                rb.velocity += moveInput;
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



            rb.velocity = currentVelocity; //* _dashMultiplier;
        }

        public void Knocked(float knockedTime, Vector3 dir) {
            knocked = knockedTime;
            IsCurrentlyDashing = true;
            power = 0.3f;
            currentVelocity = dir * 5 ;
            StartCoroutine(Dash());
        }

        public IEnumerator ChargeDash()
        {
            SoundFXManager.Instance.StopWalking();
            IsCurrentlyDashing = true;
            IsInDashState = true;
            rb.velocity = Vector2.zero;
            playerAnimator.PlayWindup(currentVelocity);
            rb.velocity = currentVelocity * 0.25f;
            power = 0;
            StartCoroutine(FlashRecharge());
            while (_chargingDash)
            {
                yield return new WaitForSeconds(0.05f);
                power = Mathf.Min(power + dashIncrease, dashMaxTime);
            }
            SoundFXManager.Instance.PlayRandomSoundFX(dash, 1f);

            power = Mathf.Max(dashMinTime, power);
            StartCoroutine(Dash());
        }

        IEnumerator FlashRecharge()
        {
            _dashImageCharger.enabled = true;
            
            while (_chargingDash)
            {
                _dashImageCharger.DOColor(new Color32(255, 255, 255, 160), 0.05f);
                yield return new WaitForSeconds(((dashMaxTime + 0.1f) - power) / 6);
                _dashImageCharger.DOColor(new Color32(255, 255, 255, 0), 0.05f);
                yield return new WaitForSeconds(((dashMaxTime + 0.1f) - power) / 6);
            }
            _dashImageCharger.enabled = false;
        }

        private IEnumerator Dash()
        {

            Vector2 velocityVector;
            IsDashing?.Invoke(true);

            IsInDashState = false;
            _dashCoolDown = 0;
            if ((Mathf.Abs(moveInput.x) > 0 || Math.Abs(moveInput.y) > 0) && knocked == 0)
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
            print(currentVelocity);
            playerAnimator.PlayDash(currentVelocity);
            yield return new WaitForSeconds(power);
            IsCurrentlyDashing = false;
            IsDashing?.Invoke(false);
            yield return new WaitForSeconds(_dashCoolDown);
        }

        public void ChangeVelocity(float multiplier)
        {
            maxSpeed *= multiplier;
            deceleration *= multiplier * 1.5f;
            acceleration *= multiplier;
            turnDeceleration *= multiplier;
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