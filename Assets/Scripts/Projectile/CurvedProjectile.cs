using UnityEngine;
using static WaveData;

namespace Projectile
{
    public class CurvedProjectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private GameObject reticlePrefab;
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private float colliderActivationPercentage = 0.9f;
        [SerializeField] private float curveHeight = 5f;

        private ShotDataObject _data;

        private Vector3 _target;
        private Vector3 _startPosition;

        private Rigidbody2D _rigidBody;
        private CapsuleCollider2D _collider;

        private GameObject _shadow;
        private GameObject _reticle;

        private float _currentTime;
        private float _damageMod = 1;
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _startPosition = transform.position;

            _collider.enabled = false;
        }

        public float GetDamageMod()
        {
            return _damageMod;
        }

        public void SetShotData(ShotDataObject shotData, Vector3 playerPosition)
        {
            _data = shotData;
            _target = playerPosition;

            if (shotData.advancedSettings.x != 0)
            {
                _target.x = shotData.advancedSettings.x;
            }

            if (shotData.advancedSettings.y != 0)
            {
                _target.y = shotData.advancedSettings.y;
            }

            _shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
            Destroy(_shadow, shotData.advancedSettings.throwAirTime);

            _reticle = Instantiate(reticlePrefab, _target, Quaternion.identity);
            Destroy(_reticle, shotData.advancedSettings.throwAirTime);
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > colliderActivationPercentage * _data.advancedSettings.throwAirTime)
            {
                _collider.enabled = true;
            }

            if (_currentTime > _data.advancedSettings.throwAirTime)
            {
                Destroy(gameObject);
            }

            var airTime = _currentTime / _data.advancedSettings.throwAirTime;
            var position = Vector2.Lerp(_startPosition, _target, airTime);

            position.y += curveHeight * _data.advancedSettings.animationCurve.Evaluate(airTime);
            _rigidBody.MovePosition(position);

            if (_shadow)
            {
                _shadow.transform.position = Vector2.Lerp(_startPosition, _target, airTime);
            }
        }

        private void OnDestroy()
        {
            Destroy(_shadow);
            Destroy(_reticle);
        }

        public ShotDataObject GetShotData()
        {
            return _data;
        }
    }
}