using Player;
using UnityEngine;

namespace Projectile
{
    public class JuggleProjectile : MonoBehaviour
    {
        [SerializeField] private GameObject reticlePrefab;
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private float colliderActivationPercentage = 0.9f;
        [SerializeField] protected float curveHeight = 5f;
        [SerializeField] private float damage;


        const float DEBOUNCE_TIME = 0.5f;
        Transform player;
        HeatSystem heatSystem;
        AudioSource sound;

        protected Rigidbody2D RigidBody;
        private Rigidbody2D _playerRb;
        private Collider2D _collider;

        protected Vector3 Target = Vector3.zero;
        protected Vector3 StartPosition;

        protected float CurrentTime;
        private GameObject _shadow;
        protected GameObject Reticle;
        protected Vector2 Direction;

        private float throwAirTime = 3f;
        public AnimationCurve animationCurve;
        public AudioClip[] Slashes;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerRb = player.GetComponent<Rigidbody2D>();
            heatSystem = player.GetComponent<HeatSystem>();
            sound = GetComponent<AudioSource>();

            RigidBody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;
            StartPosition = transform.position;

            InstantiateReticle();
        }
        private void InstantiateShadow()
        {
            _shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
            Destroy(_shadow, throwAirTime);
        }

        protected virtual void InstantiateReticle()
        {
            Reticle = Instantiate(reticlePrefab, Target, Quaternion.identity);
            Destroy(Reticle,throwAirTime);
        }

        void OnUpdate(float airTime)
        {
            if (CurrentTime > throwAirTime)
            {
                Destroy(gameObject);
            }

            var position = Vector2.Lerp(StartPosition, Target, airTime);

            position.y += curveHeight * animationCurve.Evaluate(airTime);
            RigidBody.MovePosition(position);
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;

            var airTime = CurrentTime / throwAirTime;

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

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                var closestPoint = collision.ClosestPoint(transform.position);
                collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(damage);
                //Instantiate(hitVfx, closestPoint, Quaternion.identity);
                //SoundManager.PlayHit(closestPoint);
                SoundFXManager.Instance.PlayRandomSoundFX(Slashes, transform, 1f);
                CurrentTime = 0;
                StartPosition = transform.position;
                Target = new Vector3(Random.Range(-4, 4), Random.Range(-5,3));
                throwAirTime = Mathf.Clamp((StartPosition - Target).magnitude * 0.8f, 1.5f, 3f);
                _collider.enabled = false;
                InstantiateReticle();
                return;
            }
        }
    }
}
