using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject kingDustPrefab;

        private enum State
        {
            Idle, Move, Dash, Windup, Hit
        }

        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        private Animator _animator;

        private Vector2 _lastNonZeroVelocity = Vector2.up;

        private State _nextState = State.Idle;

        private bool _doNotInterrupt;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_doNotInterrupt)
            {
                if (!(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
                {
                    _doNotInterrupt = false;
                }
                return;
            }

            switch (_nextState)
            {
                case State.Idle:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
                    _animator.Play("KingIdleAnimation");
                    break;
                case State.Move:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
                    _animator.Play("KingMoveAnimation");
                    break;
                case State.Dash:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.SetFloat(MoveY, _lastNonZeroVelocity.y);
                    _animator.Play("KingDashAnimation");
                    break;
                case State.Windup:
                    _animator.SetFloat(MoveX, _lastNonZeroVelocity.x);
                    _animator.Play("KingWindupAnimation");
                    break;
                case State.Hit:
                    var randomNumber = Random.Range(1,3);
                    _animator.Play($"KingHit_{randomNumber}");
                    _doNotInterrupt = true;
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

        public void PlayHit()
        {
            _nextState = State.Hit;
        }

        public void PlayTurning(Vector2 moveInput)
        {
            if (_lastNonZeroVelocity.x * moveInput.x < 0)
            {
                var instance = Instantiate(kingDustPrefab, transform.position, Quaternion.identity);

                var instancePosition = instance.transform.position;
                instancePosition.x = transform.position.x - (moveInput.x > 0 ? 1f : -1f);
                instancePosition.y = transform.position.y - 0.5f;

                instance.transform.position = instancePosition;

                var transformLocalScale = instance.transform.localScale;
                var absoluteScaleX = Mathf.Abs(transformLocalScale.x);
                transformLocalScale.x = moveInput.x > 0 ? absoluteScaleX : -absoluteScaleX;
                instance.transform.localScale = transformLocalScale;

                instance.GetComponent<Animator>().Play("KingTurnDust");
            }
        }
    }
}