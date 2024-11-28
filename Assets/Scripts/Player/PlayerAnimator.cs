using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        private Animator _animator;

        private Vector2 _lastNonZeroVelocity = Vector2.up;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayDash(Vector2 velocity)
        {
            _lastNonZeroVelocity = velocity;
            _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
            _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
            _animator.Play("KingDashAnimation");
        }

        public void PlayIdle()
        {
            _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
            _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
            _animator.Play("KingIdleAnimation");
        }

        public void PlayMoving(Vector2 velocity)
        {
            _lastNonZeroVelocity = velocity;
            _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
            _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
            _animator.Play("KingMoveAnimation");
        }
    }
}