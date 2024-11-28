using System;
using System.Collections;
using UnityEngine;

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
                GetComponent<SpriteRenderer>().color = col;
        }

        public void Hit()
        {
            if (_timeSinceLastHit <= comboTimeLimit)
            {
                ComboMultiplierChanged?.Invoke(_comboMultiplier);
                _comboMultiplier += comboMultiplierIncrease;
            }
            _timeSinceLastHit = 0;
        }

        public void ChangeHeat(float amount)
        {
            if (invincible)
            {
                return;
            }

            if (amount > 0)
            {
                Hit();
            }

            _currentHeat += amount * _comboMultiplier / 10;
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
                if (ColorUtility.TryParseHtmlString("#3D63FE", out Color col))
                    GetComponent<SpriteRenderer>().color = col;

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
                yield return new WaitForSeconds(comboTimeLimit);
                if (_timeSinceLastHit > comboTimeLimit)
                {
                    _comboMultiplier = 1f;
                    ComboMultiplierChanged?.Invoke(_comboMultiplier);
                }
            }
        }
    }
}