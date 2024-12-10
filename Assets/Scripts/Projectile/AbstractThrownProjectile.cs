using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Jesters.Red;

namespace Projectile
{
    public abstract class AbstractThrownProjectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private GameObject reticlePrefab;
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private float colliderActivationPercentage = 0.9f;
        [SerializeField] protected float curveHeight = 5f;

        protected RedShotDataObject Data;

        protected Vector3 Target;
        protected Vector3 StartPosition;

        protected Rigidbody2D RigidBody;
        private Collider2D _collider;

        private GameObject _shadow;
        protected GameObject Reticle;
        protected GameObject ReticleFill;

        protected float CurrentTime;
        private readonly float _damageMod = 1;
        private float _spinSpeed;
        private bool _isOn;
        protected Vector2 Direction;

        private int _numberOfBounces;
        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;
            _spinSpeed = Random.Range(1.0f, 2.0f) * (Random.Range(0, 2) * 2 - 1);
            StartPosition = transform.position;
        }

        private void Start()
        {
            Invoke(nameof(Enable), Data.fireBetween);
            InstantiateReticle(Data);
            transform.localScale *= Data.scale;
            InvokeRepeating(nameof(Spin), 0, 0.005f);
        }

        private void Enable()
        {
            _isOn = true;

            StartCoroutine(InstantiateShadow());
            GetComponent<SpriteRenderer>().enabled = true;
        }

        private void Spin()
        {
            transform.rotation *= Quaternion.Euler(0, 0, _spinSpeed);
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

        public void SetShotData(RedShotDataObject shotData, Vector3 playerPosition)
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
            if (shotData.randomY)
            {
                Target.y = Random.Range(-4.0f, 4.0f);
            }
            if (shotData.randomX)
            {
                Target.x = Random.Range(-5.0f, 4.0f);
            }
        }

        private IEnumerator InstantiateShadow()
        {
            _shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
            _shadow.transform.localScale *= Data.scale;
            Destroy(_shadow, Data.throwAirTime);

            var originalScale = _shadow.transform.localScale;
            _shadow.transform.DOScale(originalScale * 0.3f, Data.throwAirTime * 0.5f).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(Data.throwAirTime / 2);
            _shadow.transform.DOScale(originalScale, Data.throwAirTime * 0.45f).SetEase(Ease.OutQuad);
        }

        protected virtual void InstantiateReticle(RedShotDataObject shotData)
        {
            Reticle = Instantiate(reticlePrefab, Target, Quaternion.identity);
            ReticleFill = Reticle.transform.GetChild(0).gameObject;
            Reticle.transform.localScale *= Data.scale;
            ReticleFill.transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), (Data.throwAirTime + Data.fireBetween) * 1.75f);
            Destroy(Reticle, shotData.throwAirTime + shotData.fireBetween);
        }

        private void Update()
        {
            if (_isOn)
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
        }

        protected abstract void OnUpdate(float airTime);

        private void OnDestroy()
        {
            Destroy(_shadow);
            Destroy(Reticle);
        }

        public IShotData GetShotData()
        {
            return Data;
        }
    }
}
