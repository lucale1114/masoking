using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Dash = Animator.StringToHash("dash");
        private static readonly int Moving = Animator.StringToHash("moving");

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void TriggerDash()
        {
            _animator.SetTrigger(Dash);
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