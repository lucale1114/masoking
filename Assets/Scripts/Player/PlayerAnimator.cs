using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private enum State
        {
            Idle, Move, Dash, Windup
        }

        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        private Animator _animator;
        private Animator _dustAnimator;

        private Vector2 _lastNonZeroVelocity = Vector2.up;

        private State _currentState = State.Idle;
        private State _nextState = State.Idle;

        private void Start()
        {
            var animators = GetComponentsInChildren<Animator>();
            _animator = animators[0];
            _dustAnimator = animators[1];
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Idle:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
                    _animator.Play("KingIdleAnimation");
                    _currentState = _nextState;
                    break;
                case State.Move:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
                    _animator.Play("KingMoveAnimation");
                    _currentState = _nextState;
                    break;
                case State.Dash:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
                    _animator.Play("KingDashAnimation");
                    _currentState = _nextState;
                    break;
                case State.Windup:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.Play("KingWindupAnimation");
                    _currentState = _nextState;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void PlayWindup(Vector2 velocity)
        {
            _lastNonZeroVelocity = velocity;
            _nextState = State.Windup;
        }

        public void PlayDash(Vector2 velocity)
        {
            _lastNonZeroVelocity = velocity;
            _nextState = State.Dash;
        }

        public void PlayIdle()
        {
            _nextState = State.Idle;
        }

        public void PlayMoving(Vector2 velocity)
        {
            _lastNonZeroVelocity = velocity;
            _nextState = State.Move;
        }

        public void PlayTurning(Vector2 moveInput)
        {
            if (_lastNonZeroVelocity.x * moveInput.x < 0)
            {
                var transformLocalPosition = _dustAnimator.transform.localPosition;
                var absolutePositionX = Mathf.Abs(transformLocalPosition.x);
                transformLocalPosition.x = moveInput.x > 0 ? -absolutePositionX : absolutePositionX;
                _dustAnimator.transform.localPosition = transformLocalPosition;

                var transformLocalScale = _dustAnimator.transform.localScale;
                var absoluteScaleX = Mathf.Abs(transformLocalScale.x);
                transformLocalScale.x = moveInput.x > 0 ? absoluteScaleX : -absoluteScaleX;
                _dustAnimator.transform.localScale = transformLocalScale;

                _dustAnimator.Play("KingTurnDust");
            }
        }
    }
}