using System.Collections;
using UnityEngine;

namespace Player
{
    public class KingEffectsAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject kingHitAnimatorPrefab;
        [SerializeField] private GameObject kingHitVFXPrefab;
        [SerializeField] private GameObject kingComboVFXPrefab;

        public void PlayBlunt(Vector2 closestPoint)
        {
            var vfx = Instantiate(kingHitVFXPrefab, closestPoint, Quaternion.identity)
                .GetComponent<ParticleSystem>();

            var instance = Instantiate(kingHitAnimatorPrefab, closestPoint, Quaternion.identity);
            instance.transform.parent = transform;

            var animator = instance.GetComponent<Animator>();
            animator.Play("KingHit_1");

            StartCoroutine(DestroyAnimatorWhenDone(animator));
            StartCoroutine(DestroyVfxWhenDone(vfx));
        }

        public void PlaySharp(Vector2 closestPoint, Vector2 direction)
        {
            var vfx = Instantiate(kingHitVFXPrefab, closestPoint, Quaternion.identity)
                .GetComponent<ParticleSystem>();

            var instance = Instantiate(kingHitAnimatorPrefab, closestPoint, Quaternion.identity);
            instance.transform.parent = transform;

            instance.transform.right = direction;

            var animator = instance.GetComponent<Animator>();
            animator.Play("KingHit_2");

            StartCoroutine(DestroyAnimatorWhenDone(animator));
            StartCoroutine(DestroyVfxWhenDone(vfx));
        }

        public void PlayCombo(Vector2 closestPoint)
        {
            var vfx = Instantiate(kingComboVFXPrefab, closestPoint, Quaternion.identity)
                .GetComponent<ParticleSystem>();

            StartCoroutine(DestroyVfxWhenDone(vfx));
        }

        private static IEnumerator DestroyAnimatorWhenDone(Animator animator)
        {
            while (animator!.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Destroy(animator.gameObject);
        }

        private static IEnumerator DestroyVfxWhenDone(ParticleSystem vfx)
        {
            while (vfx.IsAlive())
            {
                yield return new WaitForSeconds(0.1f);
            }

            Destroy(vfx.gameObject);
        }
    }
}