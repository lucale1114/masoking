using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Jesters.Green;

namespace Jester.Green
{
    public class Movement : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");

        private Animator _animator;
        private Collision _collision;

        private GreenJesterData _data;
        private int _currentCommandIndex;

        public void SetData(GreenJesterData data)
        {
            _data = data;
            _currentCommandIndex = 0;

            _animator = GetComponent<Animator>();
            _collision = GetComponent<Collision>();

            MakeStep();
        }

        private void MakeStep()
        {
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
                case GreenJesterActions.Move:
                    _animator.SetBool(Idle, false);
                    transform.DOMove(command.destination, command.time).onComplete += () =>
                    {
                        _currentCommandIndex++;
                        MakeStep();
                    };
                    break;
                case GreenJesterActions.Idle:
                    StartCoroutine(IdleRoutine(command.time));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator IdleRoutine(float time)
        {
            _animator.SetBool(Idle, true);
            _currentCommandIndex++;
            yield return new WaitForSeconds(time);
            MakeStep();
        }

        private void Update()
        {
            if (_collision && _collision.Dashed)
            {
                transform.DOKill();
                _animator.SetBool(Idle, true);
            }
        }

        private bool IsOutOfCommands()
        {
            return _currentCommandIndex == _data.commands.Length;
        }
    }
}