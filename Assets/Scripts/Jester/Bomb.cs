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

        private void Start()
        {
            _player = GameObject.Find("Player");
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private IEnumerator AnimationExplosion(float explosionRadius, float damage)
        {
            yield return new WaitForSeconds(2.8f);

            animator.SetTrigger(Boom);

            _spriteRenderer.sortingLayerName = "InFront";

            yield return new WaitForSeconds(0.2f);

            SoundFXManager.Instance.PlayRandomSoundFX(booms, 1f);

            Exploded?.Invoke();

            if (_player)
            {
                var sqrDistance = Vector3.SqrMagnitude(transform.position - _player.transform.position);

                if (sqrDistance <= explosionRadius * explosionRadius)
                {
                    _player.GetComponent<HeatSystem>().ChangeHeat(damage);
                }
            }

            Destroy(gameObject);
        }

        public void Activate(float explosionRadius = 5f, float damage = 10f)
        {
            animator.SetBool(Count, true);

            StartCoroutine(AnimationExplosion(explosionRadius, damage));
        }
    }
}