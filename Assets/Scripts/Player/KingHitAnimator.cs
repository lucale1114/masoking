using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class KingHitAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject kingHitAnimatorPrefab;
        [SerializeField] private List<AnimationClip> kingHitAnimationClips;

        public void PlaySplash(Vector2 closestPoint)
        {
            var instance = Instantiate(kingHitAnimatorPrefab, closestPoint, Quaternion.identity);
            instance.transform.parent = transform;

            var animator = instance.GetComponent<Animator>();
            animator.Play("KingHit_1");

            StartCoroutine(DestroyWhenDone(animator));
        }

        public void PlayDirectedSplash(Vector2 closestPoint, Vector2 transformPosition)
        {
            var instance = Instantiate(kingHitAnimatorPrefab, closestPoint, Quaternion.identity);
            instance.transform.parent = transform;

            var direction = closestPoint - transformPosition;
            instance.transform.right = direction;

            var animator = instance.GetComponent<Animator>();
            animator.Play("KingHit_2");

            StartCoroutine(DestroyWhenDone(animator));
        }

        private static IEnumerator DestroyWhenDone(Animator animator)
        {
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Destroy(animator.gameObject);
        }
    }
}