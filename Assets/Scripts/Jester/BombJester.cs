using DG.Tweening;
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
        [SerializeField] private GameObject _bomb;

        private GameObject _player;
        private Animator _animator;
        private Rigidbody2D _rb;
        private BombJesterCollision _bombCollision;

        private bool _launched;
        private float _spinSpeed;

        public void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            _bomb = transform.GetChild(0).gameObject;
            _bombCollision = GetComponent<BombJesterCollision>();

            if (_bomb != null)
            {
                _bomb.transform.position = transform.position + bombOffset;
            }

            _spinSpeed = Random.Range(2f, 2.0f) * (Random.Range(0, 2) * 1.5f - 1);
        }

        public void Update()
        {
            /* if (!_launched)
             {
                 if (_bomb)
                 {
                     _bomb.transform.position = transform.position + bombOffset;
                 }
             }

             else
             {
                 transform.rotation *= Quaternion.Euler(0, 0, _spinSpeed);

             }

             if (_bombCollision.HasDashed && !_launched)
             {
                 _animator.SetBool(Idle, true);
                 SoundFXManager.Instance.PlayRandomSoundFX(smack, 1f);

                 _launched = true;
                 Launch();
             }*/



            if (!_launched)
            {
                if (_bomb)
                {
                    // Only update the bomb's position if it's still attached
                    _bomb.transform.position = transform.position + bombOffset;
                }
            }
            else
            {
                // Rotate the parent object when launched
                transform.rotation *= Quaternion.Euler(0, 0, _spinSpeed);
            }

            // Trigger launch when the player dashes into the object
            if (_bombCollision.HasDashed && !_launched)
            {
                _animator.SetBool(Idle, true);
                SoundFXManager.Instance.PlayRandomSoundFX(smack, 1f);
                _launched = true;
                Launch();
            }




        }

        private void Launch()
        {
            _bomb.transform.position = Vector3.zero;
            transform.DOKill();
            var launchDirection = (transform.position - _player.transform.position).normalized;
            _rb.velocity = thrust * launchDirection;
            if (_bomb)
            {
                _bomb.transform.position = transform.position + bombOffset;
                _bomb.transform.parent = null;
            }

            Destroy(gameObject, 2f);
        }
    }

}