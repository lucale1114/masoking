using UnityEngine;

namespace Jester
{
    public class JesterAnimator : MonoBehaviour
    {
        private static readonly int Moving = Animator.StringToHash("moving");
        private static readonly int Fire = Animator.StringToHash("fire");

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void TriggerFire()
        {
            _animator.SetTrigger(Fire);
        }

        public void SetIdle()
        {
            _animator.SetBool(Moving, false);
        }

        public void SetMoving()
        {
            _animator.SetBool(Moving, true);
        }
    }
}