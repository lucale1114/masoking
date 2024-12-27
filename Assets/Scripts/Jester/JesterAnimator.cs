using Player;
using System.Collections;
using UnityEngine;
using Misc;
using Wave.Handler;

namespace Jester
{
    public class JesterAnimator : MonoBehaviour
    {
        private static readonly int Moving = Animator.StringToHash("moving");
        private static readonly int Fire = Animator.StringToHash("fire");
        [SerializeField] private AudioClip[] discust;
        [SerializeField, Range(0f,1f)] float chance = 0.8f;
        private WaveHandler _waveHandler;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _waveHandler = FindObjectOfType<WaveHandler>();
        }

        public void TriggerFire()
        {
            _animator.SetTrigger(Fire);
            if (!_waveHandler.End == true)
            {
                StartCoroutine(SoundChance());

            }
        }

        public void SetIdle()
        {
            _animator.SetBool(Moving, false);
        }

        public void SetMoving()
        {
            _animator.SetBool(Moving, true);
        }

        public IEnumerator SoundChance()
        {
            float roll = Random.value;
            if (roll < chance)
            {
                yield return new WaitForSeconds(1);
                SoundFXManager.Instance.PlayRandomSoundFXNoPitch(discust, 0.7f);
            }


        }
    }
}