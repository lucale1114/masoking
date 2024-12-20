using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Jester
{
    public class Bomb : MonoBehaviour
    {
        private static readonly int Boom = Animator.StringToHash("boom");
        private static readonly int Count = Animator.StringToHash("count");

        public static Action Exploded;

        [SerializeField] private AudioClip[] booms;
        [SerializeField] private Animator animator;

        private GameObject _player;
        private SpriteRenderer _spriteRenderer;

        private static int movementDisableCounter = 0; // Counter for active bombs disabling movement


        private void Start()
        {
            _player = GameObject.Find("Player");
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(ReenableMovementAfterDelay());

        }

        private IEnumerator AnimationExplosion(float explosionRadius, float damage)
        {
            yield return new WaitForSeconds(0.5f);

            animator.SetTrigger(Boom);

            _spriteRenderer.sortingLayerName = "InFront";

            SoundFXManager.Instance.PlayRandomSoundFX(booms, 1f);

            yield return new WaitForSeconds(0.2f);

            Exploded?.Invoke();

            if (_player)
            {
                var sqrDistance = Vector3.SqrMagnitude(transform.position - _player.transform.position);

                if (sqrDistance <= explosionRadius * explosionRadius)
                {
                    _player.GetComponent<HeatSystem>().ChangeHeat(damage);

                    movementDisableCounter++;
                    _player.GetComponent<Player.Movement>().enabled = false;
                    // Apply explosion force
                    Rigidbody2D playerRb = _player.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        Vector2 forceDirection = _player.transform.position - transform.position;
                         forceDirection.Normalize();
                         float explosionForce = 2f; // Adjust as needed
                        playerRb.AddForce(forceDirection * explosionForce, ForceMode2D.Impulse);


                    }


                }
            }

            Destroy(gameObject);
        }

        private IEnumerator ReenableMovementAfterDelay()
        {
            UnityEngine.Debug.Log("hell");

            yield return new WaitForSeconds(2f); // Delay for explosion effect

            // Decrement the counter and re-enable movement if no more bombs are active
            movementDisableCounter--;

            UnityEngine.Debug.Log("FUCK");

            if (movementDisableCounter <= 0)
            {
                movementDisableCounter = 0; // Ensure it never goes negative
                _player.GetComponent<Player.Movement>().enabled = true;
            }
        }

        public void Activate(float explosionRadius = 5f, float damage = 25f)
        {
            animator.SetBool(Count, true);

            StartCoroutine(AnimationExplosion(explosionRadius, damage));
        }
    }
}