using Player;
using Projectile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float shakeAmount = 0.7f;
        [SerializeField] private float shakeDuration = 0.2f;

        private Camera _cam;

        private bool _shake;

        private Vector3 _startPosition;

        private float _shakeTimer;
        private float _shakeStrength;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            _startPosition = _cam!.transform.localPosition;
            _shakeTimer = shakeDuration;
            _shakeStrength = shakeAmount;
        }

        private void Start()
        {
            FindObjectOfType<HeatSystem>().TakenDamage += () =>
            {
                ShakeCamera(shakeDuration, shakeAmount);
            };
            FindObjectOfType<Movement>().Bounced += (bool a, Vector3 b) =>
            { 
                ShakeCamera(shakeDuration, shakeAmount * 2);
            };
            Jester.Bomb.Exploded += () =>
            {
                ShakeCamera(shakeDuration, shakeAmount);
            };
            BallProjectile.Landed += () =>
            {
                ShakeCamera(shakeDuration, shakeAmount);
            };
        }

        private void ShakeCamera(float duration, float strength)
        {
            _shakeTimer = duration * 2;
            _shakeStrength = strength * 2;
            _shake = true;
        }

        private void Update()
        {
            if (_shake)
            {
                if (_shakeTimer > 0)
                {
                    var randomPosition = _startPosition + Random.insideUnitSphere * _shakeStrength;
                    _cam.transform.localPosition =
                        Vector3.Lerp(_cam.transform.localPosition, randomPosition, Time.deltaTime);

                    _shakeTimer -= Time.deltaTime;
                }
                else
                {
                    _cam.transform.localPosition = _startPosition;
                    _shake = false;
                }
            }
        }
    }
}