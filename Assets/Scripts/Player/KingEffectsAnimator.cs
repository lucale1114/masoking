using System.Collections;
using UnityEngine;

namespace Player
{
    public class KingEffectsAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject bloodAnimatorPrefab;
        [SerializeField] private GameObject hitAnimatorPrefab;
        [SerializeField] private GameObject kingHitVFXPrefab;
        [SerializeField] private GameObject kingComboVFXPrefab;

        public void PlayBlunt(Vector2 closestPoint)
        {
            var vfx = Instantiate(kingHitVFXPrefab, closestPoint, Quaternion.identity)
                .GetComponent<ParticleSystem>();

            StartCoroutine(DestroyVfxWhenDone(vfx));

            var blood = Instantiate(bloodAnimatorPrefab, closestPoint, Quaternion.identity);
            blood.transform.parent = transform;

            var bloodAnimator = blood.GetComponent<Animator>();
            bloodAnimator.Play("Blood_1");

            StartCoroutine(DestroyAnimatorWhenDone(bloodAnimator));

            var hit = Instantiate(hitAnimatorPrefab, closestPoint, Quaternion.identity);
            hit.transform.parent = transform;

            var hitAnimator = hit.GetComponent<Animator>();
            hitAnimator.Play($"Hit_Blunt_{Random.Range(1, 2)}");

            StartCoroutine(DestroyAnimatorWhenDone(hitAnimator));
        }

        public void PlaySharp(Vector2 closestPoint, Vector2 direction)
        {
            var vfx = Instantiate(kingHitVFXPrefab, closestPoint, Quaternion.identity)
                .GetComponent<ParticleSystem>();

            StartCoroutine(DestroyVfxWhenDone(vfx));

            var blood = Instantiate(bloodAnimatorPrefab, closestPoint, Quaternion.identity);
            blood.transform.parent = transform;

            blood.transform.right = direction;

            var bloodAnimator = blood.GetComponent<Animator>();
            bloodAnimator.Play("Blood_2");

            StartCoroutine(DestroyAnimatorWhenDone(bloodAnimator));

            var hit = Instantiate(hitAnimatorPrefab, closestPoint, Quaternion.identity);
            hit.transform.parent = transform;

            hit.transform.right = -direction;

            var hitAnimator = hit.GetComponent<Animator>();
            hitAnimator.Play($"Hit_Sharp_1");

            StartCoroutine(DestroyAnimatorWhenDone(hitAnimator));
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