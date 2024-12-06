using DG.Tweening;
using UnityEngine;
using Wave;

namespace Jester.Green
{
    public class Movement : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");

        private Animator _animator;
        private Collision _collision;

        private WaveData.MovingJesterData _data;
        private int _currentCommandIndex;

        public void Start()
        {
            _animator = GetComponent<Animator>();
            _collision = GetComponent<Collision>();

            _currentCommandIndex = 0;
        }

        public void SetData(WaveData.MovingJesterData data)
        {
            _data = data;
            MakeStep();
        }

        private void MakeStep()
        {
            if (IsOutOfCommands())
            {
                Destroy(gameObject);
            }

            var command = _data.commands[_currentCommandIndex];
            var direction = command.destination - (Vector2) transform.position;

            switch (direction.x)
            {
                case > 0 when transform.localScale.x > 0:
                case < 0 when transform.localScale.x < 0:
                    var transformLocalScale = transform.localScale;
                    transformLocalScale.x *= -1;
                    transform.localScale = transformLocalScale;
                    break;
            }
            transform.DOMove(command.destination, command.time).onComplete += () =>
            {
                _currentCommandIndex++;
                MakeStep();
            };
        }

        private void Update()
        {
            if (_collision.Dashed)
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