using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject kingDustPrefab;

        private enum State
        {
            Idle, Move, Dash, Windup, Hit, Combo, Turn, Relaxing
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
                    _animator.Play($"KingHit_{Random.Range(1,4)}");
                    _doNotInterrupt = true;
                    _nextState = State.Idle;
                    break;
                case State.Combo:
                    StartCoroutine(ComboRoutine());
                    break;
                case State.Turn:
                    _animator.SetFloat(MoveX, -_lastNonZeroVelocity.x);
                    _animator.Play("KingWindupAnimation");
                    _doNotInterrupt = true;
                    _nextState = State.Idle;
                    break;
                case State.Relaxing:
                    _animator.Play("KingRelaxing");
                    _doNotInterrupt = true;
                    _nextState = State.Relaxing;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator ComboRoutine()
        {
            yield return new WaitForSeconds(0.75f);
            _animator.Play($"KingCombo_{Random.Range(1,3)}");
            _doNotInterrupt = true;
            _nextState = State.Idle;
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
            if (!_doNotInterrupt)
            {
                _nextState = State.Hit;
            }
        }

        public void PlayRelax()
        {
            if (!_doNotInterrupt)
            {
                _nextState = State.Relaxing;
            }
        }

        public void PlayCombo()
        {
            if (!_doNotInterrupt)
            {
                _nextState = State.Combo;
            }
        }

        public void PlayTurning(Vector2 moveInput)
        {
            if (_lastNonZeroVelocity.x * moveInput.x < 0)
            {
                if (_nextState != State.Hit && _nextState != State.Combo)
                {
                    _nextState = State.Turn;
                }

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