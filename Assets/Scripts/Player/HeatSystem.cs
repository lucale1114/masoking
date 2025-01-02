using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;

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
        public event Action<float> TakenDamage;
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
        private KingEffectsAnimator _kingEffectsAnimator;
        private ParticleSystem maxHeatEffect;

        private float _currentHeat;
        private float _timeSinceLastHit;
        private float _comboMultiplier = 0f;
        private Movement _movement;
        private Score _score;
        private float realHeatDecay;
        public bool beenHit = false;
        private bool _comboAnimationTriggered;

        private HealthBar healthBar; // Reference to the HealthBar script

        private void Start()
        {
            _currentHeat = startHeat;
            _movement = GetComponent<Movement>();
            _score = FindObjectOfType<Score>();
            _animator = GetComponent<PlayerAnimator>();
            _kingEffectsAnimator = GetComponent<KingEffectsAnimator>();
            maxHeatEffect = transform.Find("Max_Heat_Aura").GetComponent<ParticleSystem>();
            maxHeatEffect.Stop();
            realHeatDecay = heatDecayPerSecond / 100;
            healthBar = FindObjectOfType<HealthBar>(); // Find the HealthBar in the scene
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

        public void ChangeHeat(float amount, HeatSource heatSource = HeatSource.None, Vector2 impactPoint = default, Vector2 damageDirection = default)
        {
            if (invincible)
            {
                return;
            }

            // Handle heat sources and other effects (existing code remains unchanged)
            switch (heatSource)
            {
                case HeatSource.None:
                    break;
                case HeatSource.Sharp:
                    _kingEffectsAnimator.PlaySharp(impactPoint, damageDirection);
                    break;
                case HeatSource.Blunt:
                    _kingEffectsAnimator.PlayBlunt(impactPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(heatSource), heatSource, null);
            }

            // Combo and score updates (existing code remains unchanged)
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

            // Update the heat value
            _currentHeat += amount * combo;
            _currentHeat = Mathf.Clamp(_currentHeat, 0, maximumHeat);

            // Update the health bar (call to the HealthBar script)
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(GetCurrentHeatNormalized());
            }

            HeatChanged?.Invoke(GetCurrentHeatNormalized());

            // If heat is depleted
            if (_currentHeat <= 0)
            {
                HeatDepleted?.Invoke();
                return;
            }

            // If heat is increased
            if (amount > 0)
            {
                TakenDamage?.Invoke(amount);
                beenHit = true;
            }

            // Max heat handling
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
                        maxHeatEffect.Play();

                }

                MaxHeat?.Invoke();
                StartCoroutine(MaxHeatReward());
            }

            if (amount > 0)
            {
                _animator.PlayHit();
            }
        }

        public void PlayCombo()
        {
            _animator.PlayCombo();
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
                yield return new WaitForSeconds(0.01f);
                ChangeHeat(-realHeatDecay);
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
                    _comboAnimationTriggered = false;
                }
            }
        } 

        private IEnumerator MaxHeatReward()
        {
            // Example effect when heat is at max:
            _movement.ChangeVelocity(2);  // Boost player speed for 5 seconds
            yield return new WaitForSeconds(5);
            _movement.ChangeVelocity(0.5f); // Reset to normal speed

            // Example of color change on reaching max heat (optional):
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color col))
            {   
                GetComponent<SpriteRenderer>().DOColor(col, 1);
                maxHeatEffect.Stop();
            }
        }
    }
}
