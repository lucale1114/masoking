using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;
using Color = UnityEngine.Color;

namespace Player
{
    public class HeatSystem : MonoBehaviour
    {
        public enum HeatSource
        {
            None,
            Sharp,
            Blunt
        }

        public event Action HeatDepleted;
        public event Action<float> HeatChanged;
        public event Action TakenDamage;
        public event Action<float> ComboMultiplierChanged;
        public event Action MaxHeat;
        public event Action<float> ComboEnded;

        [SerializeField] private float maximumHeat = 100;
        [SerializeField] private float startHeat = 50;
        [SerializeField] private float heatDecayPerSecond = 4;
        [SerializeField] private float comboTimeLimit = 1;
        [SerializeField] private float comboMultiplierIncrease = 1f;

        public bool invincible;
        public bool CanMaxHeat = true;

        private PlayerAnimator _animator;
        private KingHitAnimator _kingHitAnimator;

        private float _currentHeat;

        private float _timeSinceLastHit;
        private float _comboMultiplier = 0f;
        private Movement _movement;
        private Score _score;


        private void Start()
        {
            _currentHeat = startHeat;
            _movement = GetComponent<Movement>();
            _score = FindObjectOfType<Score>();
            _animator = GetComponent<PlayerAnimator>();
            _kingHitAnimator = GetComponent<KingHitAnimator>();

            StartCoroutine(HeatDecayRoutine());
            StartCoroutine(ComboDecayRoutine());
        }

        private void Update()
        {
            if (_comboMultiplier != 0)
            {
                _timeSinceLastHit += Time.deltaTime;
            }
        }
        private IEnumerator MaxHeatReward()
        {
            _movement.ChangeVelocity(2);
            yield return new WaitForSeconds(5);
            _movement.ChangeVelocity(1/2f);

            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color col) && !JesterFeverHandler.JesterFever)
                GetComponent<SpriteRenderer>().DOColor(col, 1);
        }

        public void ChangeHeat(float amount,
            HeatSource heatSource = HeatSource.None,
            Vector2 impactPoint = default,
            Vector2 damageDirection = default)
        {
            if (invincible)
            {
                return;
            }

            switch (heatSource)
            {
                case HeatSource.None:
                    break;
                case HeatSource.Sharp:
                    _kingHitAnimator.PlayDirectedSplash(impactPoint, damageDirection);
                    break;
                case HeatSource.Blunt:
                    _kingHitAnimator.PlaySplash(impactPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(heatSource), heatSource, null);
            }

            if (amount > 0)
            {
                _animator.PlayHit();
            }

            float combo = 1;
            if (amount > 0)
            {
                if (_timeSinceLastHit <= comboTimeLimit)
                {
                    _comboMultiplier += comboMultiplierIncrease;
                    ComboMultiplierChanged?.Invoke(_comboMultiplier);
                    combo = (_comboMultiplier / 10) + 1;
                    _score.AddScore(amount * 5 * (combo * 10));
                }

                _timeSinceLastHit = 0;
            }

            _currentHeat += amount * combo;
            _currentHeat = Mathf.Clamp(_currentHeat, 0, maximumHeat);
            HeatChanged?.Invoke(GetCurrentHeatNormalized());

            if (_currentHeat <= 0)
            {
                HeatDepleted?.Invoke();
                return;
            }

            if (amount > 0)
            {
                TakenDamage?.Invoke();
            }

            if (_currentHeat >= maximumHeat && CanMaxHeat)
            {
                if (JesterFeverHandler.JesterFever)
                {
                    if (ColorUtility.TryParseHtmlString("#3D63FE", out Color col))
                        GetComponent<SpriteRenderer>().DOColor(col, 1);
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString("#FF0000", out Color col))
                        GetComponent<SpriteRenderer>().DOColor(col, 1);
                }


                //MaxHeat?.Invoke();
                //StartCoroutine(MaxHeatReward());
            }
        }

        public float GetCombo()
        {
            return _comboMultiplier;
        }

        public float GetCurrentHeatNormalized()
        {
            return _currentHeat / maximumHeat;
        }

        private IEnumerator HeatDecayRoutine()
        {
            ChangeHeat(0);
            while (true)
            {
                yield return new WaitForSeconds(1f);
                ChangeHeat(-heatDecayPerSecond);
            }
        }

        private IEnumerator ComboDecayRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (_timeSinceLastHit > comboTimeLimit && _comboMultiplier != 0f)
                {
                    ComboMultiplierChanged?.Invoke(0f);
                    ComboEnded?.Invoke(_comboMultiplier);
                    _comboMultiplier = 0f;
                    _timeSinceLastHit = 0f;
                }
            }
        }
    }
}