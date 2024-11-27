using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayDash(float x, float y)
        {
            _animator.SetFloat(MoveX, x);
            _animator.SetFloat(MoveY, y);
            _animator.Play("KingDashAnimation");
        }

        public void PlayIdle(float x, float y)
        {
            _animator.SetFloat(MoveX, x);
            _animator.SetFloat(MoveY, y);
            _animator.Play("KingIdleAnimation");
        }

        public void PlayMoving(float x, float y)
        {
            _animator.SetFloat(MoveX, x);
            _animator.SetFloat(MoveY, y);
            _animator.Play("KingMoveAnimation");
        }
    }
}