using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class HeatSystem : MonoBehaviour
    {
        public event Action HeatDepleted;
        public event Action<float> HeatChanged;
        public event Action TakenDamage;
        public event Action<float> ComboMultiplierChanged;

        [SerializeField] private float maximumHeat = 150;
        [SerializeField] private float startHeat = 50;
        [SerializeField] private float heatDecayPerSecond = 1;
        [SerializeField] private float comboTimeLimit = 1;
        [SerializeField] private float comboMultiplierIncrease = .1f;
        [SerializeField] private float dashHeatCost = 5;

        public bool invincible;

        private float _currentHeat;

        private float _timeSinceLastHit;
        private float _comboMultiplier = 1f;

        private void Start()
        {
            _currentHeat = startHeat;
            GetComponent<Movement>().IsDashing += isDashing =>
            {
                if (isDashing)
                {
                    ChangeHeat(-dashHeatCost);
                }
            };
            StartCoroutine(HeatDecayRoutine());
            StartCoroutine(ComboDecayRoutine());
        }

        private void Update()
        {
            _timeSinceLastHit += Time.deltaTime;
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

            _currentHeat += amount * _comboMultiplier;
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

            if (_comboMultiplier >= 2)
            {
                ComboMultiplierChanged?.Invoke(_comboMultiplier);
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