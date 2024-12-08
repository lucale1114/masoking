using Objects;
using Player;
using UnityEngine;

namespace Jester
{
    public class BombJester : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");

        [SerializeField] private float thrust = 15;
        [SerializeField] private AudioClip[] smack;
        [SerializeField] private Vector3 bombOffset = new(0.175f, 0.67f, 0); // Offset for bomb position

        private GameObject _player;
        private Animator _animator;
        private GameObject _bomb;
        private Rigidbody2D _rb;
        private BombJesterCollision _bombCollision;

        private bool _launched;
        private float _spinSpeed;

        public void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _bomb = GameObject.FindGameObjectWithTag("Bomb");
            _bombCollision = GetComponent<BombJesterCollision>();

            if (_bomb != null)
            {
                _bomb.transform.position = transform.position + bombOffset;
                _bomb.transform.parent = null;
            }

            _spinSpeed = Random.Range(20f, 20.0f) * (Random.Range(0, 2) * 2 - 1);
        }

        public void Update()
        {
            if (!_launched)
            {
                if (_bomb)
                {
                    _bomb.transform.position = transform.position + bombOffset;
                }
            }

            if (_bombCollision.HasDashed && !_launched)
            {
                _animator.SetBool(Idle, true);
                SoundFXManager.Instance.PlayRandomSoundFX(smack, transform, 1f);
                transform.rotation *= Quaternion.Euler(0, 0, _spinSpeed);
                _launched = true;
                Launch();
            }
        }

        private void Launch()
        {
            _bomb.transform.position = Vector3.zero;
            var launchDirection = (transform.position - _player.transform.position).normalized;
            _rb.velocity = thrust * launchDirection;
            if (_bomb)
            {
                _bomb.transform.position = transform.position + bombOffset;
                _bomb = null;
            }

            Destroy(gameObject, 4f);
        }
    }
}