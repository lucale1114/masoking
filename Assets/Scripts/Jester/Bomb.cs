using System.Collections;
using Player;
using UnityEngine;

namespace Jester
{
    public class Bomb : MonoBehaviour
    {
        private static readonly int Boom = Animator.StringToHash("boom");
        private static readonly int Count = Animator.StringToHash("count");

        [SerializeField] private AudioClip[] booms;
        [SerializeField] private Animator animator;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private float damage = 10f;

        private GameObject _player;

        private void Start()
        {
            _player = GameObject.Find("Player");
        }

        private IEnumerator AnimationExplosion()
        {
            yield return new WaitForSeconds(2.8f);
            animator.SetBool(Boom, true);
        }

        public void WaitForExplosion()
        {
            SoundFXManager.Instance.PlayRandomSoundFX(booms, 1f);

            if (_player != null)
            {
                var sqrDistance = Vector3.SqrMagnitude(transform.position - _player.transform.position);

                if (sqrDistance <= explosionRadius * explosionRadius)
                {
                    _player.GetComponent<HeatSystem>().ChangeHeat(damage);
                }
            }

            Destroy(gameObject);
        }

        public void SetProperties(float explosionRadius, float damage)
        {
            this.explosionRadius = explosionRadius;
            this.damage = damage;
        }

        public void Activate()
        {
            animator.SetBool(Count, true);
            StartCoroutine(AnimationExplosion());
            Invoke(nameof(WaitForExplosion), 3f);
        }
    }
}