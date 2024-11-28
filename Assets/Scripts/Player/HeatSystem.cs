using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using static WaveData;

namespace Player
{
    public class HeatSystem : MonoBehaviour
    {
        public event Action HeatDepleted;
        public event Action<float> HeatChanged;
        public event Action TakenDamage;
        public event Action<float> ComboMultiplierChanged;
        public event Action MaxHeat;

        [SerializeField] private float maximumHeat = 100;
        [SerializeField] private float startHeat = 50;
        [SerializeField] private float heatDecayPerSecond = 4;
        [SerializeField] private float comboTimeLimit = 1;
        [SerializeField] private float comboMultiplierIncrease = 1f;

        public bool invincible;
        public bool CanMaxHeat = true;

        private float _currentHeat;

        private float _timeSinceLastHit;
        private float _comboMultiplier = 1f;
        private Movement _movement;

        private void Start()
        {
            _currentHeat = startHeat;
            _movement = GetComponent<Movement>();

            StartCoroutine(HeatDecayRoutine());
            StartCoroutine(ComboDecayRoutine());
        }

        private void Update()
        {
            _timeSinceLastHit += Time.deltaTime;
        }
        private IEnumerator MaxHeatReward()
        {
            switch(UnityEngine.Random.Range(0,2))
            {
                case 0:
                    _movement.DashFest(true);
                    yield return new WaitForSeconds(5);

                    _movement.DashFest(false);
                    break;

                case 1:
                    _movement.ChangeVelocity(3);
                    yield return new WaitForSeconds(5);
                    _movement.ChangeVelocity(1/3f);
                    break;
            }

            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color col))
                GetComponent<SpriteRenderer>().DOColor(col, 1);
        }

        public void ChangeHeat(float amount)
        {
            if (invincible)
            {
                return;
            }
            if (amount > 0)
            {
                if (_timeSinceLastHit <= comboTimeLimit)
                {
                    _comboMultiplier += comboMultiplierIncrease;
                    ComboMultiplierChanged?.Invoke(_comboMultiplier);
                }

                _timeSinceLastHit = 0;
            }

            _currentHeat += amount * ((_comboMultiplier / 10) + 1);
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

            ComboMultiplierChanged?.Invoke(_comboMultiplier);
            if (_currentHeat >= maximumHeat && CanMaxHeat)
            {   
                if (JesterFever)
                {
                    if (ColorUtility.TryParseHtmlString("#3D63FE", out Color col))
                        GetComponent<SpriteRenderer>().DOColor(col, 1);
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString("#FF0000", out Color col))
                        GetComponent<SpriteRenderer>().DOColor(col, 1);
                }
                    

                MaxHeat?.Invoke();
                StartCoroutine(MaxHeatReward());
            }
        }

        private float GetCurrentHeatNormalized()
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
            ComboMultiplierChanged?.Invoke(_comboMultiplier);
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (_timeSinceLastHit > comboTimeLimit)
                {
                    _comboMultiplier = 1f;
                    ComboMultiplierChanged?.Invoke(_comboMultiplier);
                }
            }
        }
    }
}