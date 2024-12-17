using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class KingHitAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject kingHitAnimatorPrefab;
        [SerializeField] private List<AnimationClip> kingHitAnimationClips;

        public void Play(Vector2 closestPoint)
        {
            var instance = Instantiate(kingHitAnimatorPrefab, closestPoint, Quaternion.identity);
            instance.transform.parent = transform;

            var animator = instance.GetComponent<Animator>();
            animator.Play(kingHitAnimationClips[Random.Range(0, kingHitAnimationClips.Count)].name);

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