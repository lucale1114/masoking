using System;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;
using Wave.Jesters;

namespace Jester
{
    public abstract class AbstractStandingJesterBehaviour<T> : MonoBehaviour where T : IHasTimestamp
    {
        public static event Action<GameObject> AnyJesterDestroyed;

        public float enterTimestamp;
        public T[] jesterCommands;

        protected float LeaveTime;

        protected GameObject Player;
        protected JesterAnimator JesterAnimator;

        private int _dir;
        private float _currentTick = -1;
        private bool _isMoving;

        protected void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            JesterAnimator = GetComponent<JesterAnimator>();

            if (transform.position.x < 0)
            {
                FlipDirection();
                _dir = 1;
            }
            else
            {
                _dir = -1;
            }

            CalculateLeaveTime();
        }

        protected abstract void CalculateLeaveTime();

        private void FlipDirection()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        protected abstract void OnCommandTime(T command);

        private void Update()
        {
            if (!Mathf.Approximately(_currentTick, WaveHandler.Timestamp))
            {
                _currentTick = WaveHandler.Timestamp;

                if (_isMoving)
                {
                    return;
                }

                foreach (T command in jesterCommands)
                {
                    if (Mathf.Approximately(enterTimestamp, WaveHandler.Timestamp))
                    {
                        MoveIntoView();
                        return;
                    }

                    if (Mathf.Approximately(command.GetTimestamp(), WaveHandler.Timestamp))
                    {
                        OnCommandTime(command);
                    }
                }

                if (Mathf.Approximately(LeaveTime, WaveHandler.Timestamp))
                {
                    LeaveView();
                }
            }
        }

        private void MoveIntoView()
        {
            _isMoving = true;
            JesterAnimator.SetMoving();
            transform.DOLocalMoveX(transform.position.x + 2 * _dir, StandingJesterHandler.WalkingIn).onComplete +=
                () =>
                {
                    _isMoving = false;
                    JesterAnimator.SetIdle();
                };
        }

        private void LeaveView()
        {
            _isMoving = true;
            JesterAnimator.SetMoving();
            transform.DOLocalMoveX(transform.position.x + 2 * -_dir, StandingJesterHandler.WalkingOut).onComplete +=
                () =>
                {
                    AnyJesterDestroyed?.Invoke(gameObject);
                    Destroy(gameObject, 1.5f);
                };
        }
    }
}