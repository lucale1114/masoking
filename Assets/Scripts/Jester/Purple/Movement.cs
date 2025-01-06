using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Collision = Jester.Green.Collision;

namespace Jester.Purple
{
    public class Movement : MonoBehaviour
    {
        public static event Action<GameObject> AnyJesterDestroyed;

        private static readonly int Idle = Animator.StringToHash("Idle");

        private Animator _animator;
        private Collision _collision;

        private PurpleJesterData _data;
        private int _currentCommandIndex;
        private bool _interrupted; // To check if movement is interrupted

        public void SetData(PurpleJesterData data)
        {
            _data = data;
            _currentCommandIndex = 0;

            _animator = GetComponent<Animator>();
            _collision = GetComponent<Collision>();

            MakeStep();
        }

        private void MakeStep()
        {
            // Check if movement is interrupted, stop further actions
            if (_interrupted) return;

            if (IsOutOfCommands())
            {
                if (_data.loop)
                {
                    _currentCommandIndex = 0;
                }
                else
                {
                    Destroy(gameObject);
                }
                return;
            }

            var command = _data.commands[_currentCommandIndex];
            var direction = command.destination - (Vector2)transform.position;

            switch (direction.x)
            {
                case > 0 when transform.localScale.x > 0:
                case < 0 when transform.localScale.x < 0:
                    var transformLocalScale = transform.localScale;
                    transformLocalScale.x *= -1;
                    transform.localScale = transformLocalScale;
                    break;
            }

            switch (command.action)
            {
                case PurpleJesterActions.Move:
                    _animator.SetBool(Idle, false);
                    transform.DOMove(command.destination, command.time)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            _currentCommandIndex++;
                            MakeStep();
                        });
                    break;
                case PurpleJesterActions.Idle:
                    StartCoroutine(IdleRoutine(command.time));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator IdleRoutine(float time)
        {
            // Set Idle animation and wait for the idle time
            _animator.SetBool(Idle, true);
            _currentCommandIndex++;
            yield return new WaitForSeconds(time);

            // Check if interrupted before resuming
            if (!_interrupted)
            {
                MakeStep();
            }
        }

        private void Update()
        {
            // Check if Jester has been dashed into and interrupt its movement
            if (_collision && _collision.Dashed && !_interrupted)
            {
                InterruptMovement();
            }
        }

        private void InterruptMovement()
        {
            _interrupted = true; // Mark as interrupted
            transform.DOKill(); // Stop any DOTween-based movement
            _animator.SetBool(Idle, true); // Set to idle animation

            // Optional: Add additional logic if needed, such as disabling other movement scripts
        }

        private bool IsOutOfCommands()
        {
            return _currentCommandIndex >= _data.commands.Length;
        }

        private void OnDestroy()
        {
            AnyJesterDestroyed?.Invoke(gameObject);
        }
    }
}
