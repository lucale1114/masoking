using UnityEngine;
using static WaveData;

namespace Projectile
{
    public abstract class AbstractThrownProjectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private GameObject reticlePrefab;
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private float colliderActivationPercentage = 0.9f;
        [SerializeField] protected float curveHeight = 5f;

        protected ShotDataObject Data;

        protected Vector3 Target;
        protected Vector3 StartPosition;

        protected Rigidbody2D RigidBody;
        private Collider2D _collider;

        private GameObject _shadow;
        protected GameObject Reticle;

        protected float CurrentTime;
        private readonly float _damageMod = 1;

        protected Vector2 Direction;

        private int _numberOfBounces;

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;

            StartPosition = transform.position;
        }

        public float GetDamageMod()
        {
            return _damageMod;
        }

        public bool CanHitThings()
        {
            return _collider.enabled;
        }

        public int GetNumberOfBounces()
        {
            return _numberOfBounces;
        }

        public void AttemptBounce(Vector2 normal)
        {
            if (!(normal.x * Direction.x >= 0 && normal.y * Direction.y >= 0))
            {
                _numberOfBounces--;
                Direction = Vector2.Reflect(Direction, normal);
            }
        }

        public void SetShotData(ShotDataObject shotData, Vector3 playerPosition)
        {
            Data = shotData;
            Target = playerPosition;
            _numberOfBounces = Data.numberOfBounces;

            if (shotData.x != 0)
            {
                Target.x = shotData.x;
            }

            if (shotData.y != 0)
            {
                Target.y = shotData.y;
            }

            InstantiateShadow(shotData);
            InstantiateReticle(shotData);
        }

        private void InstantiateShadow(ShotDataObject shotData)
        {
            _shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
            Destroy(_shadow, shotData.throwAirTime);
        }

        protected virtual void InstantiateReticle(ShotDataObject shotData)
        {
            Reticle = Instantiate(reticlePrefab, Target, Quaternion.identity);
            Destroy(Reticle, shotData.throwAirTime);
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;

            var airTime = CurrentTime / Data.throwAirTime;

            if (airTime >= colliderActivationPercentage)
            {
                _collider.enabled = true;
            }

            OnUpdate(airTime);

            if (_shadow)
            {
                _shadow.transform.position = Vector2.Lerp(StartPosition, Target, airTime);
            }
        }

        protected abstract void OnUpdate(float airTime);

        private void OnDestroy()
        {
            Destroy(_shadow);
            Destroy(Reticle);
        }

        public ShotDataObject GetShotData()
        {
            return Data;
        }
    }
}