using Player;
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

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            _startPosition = _cam!.transform.localPosition;
            _shakeTimer = shakeDuration;
        }

        private void Start()
        {
            FindObjectOfType<HeatSystem>().TakenDamage += () =>
            {
                _shakeTimer = shakeDuration;
                _shake = true;
            };
        }

        private void Update()
        {
            if (_shake)
            {
                if (_shakeTimer > 0)
                {
                    var randomPosition = _startPosition + Random.insideUnitSphere * shakeAmount;
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