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
        protected JesterFire JesterFire;
        protected GameObject Player;
        protected JesterAnimator JesterAnimator;
        protected LineRenderer LineRenderer;

        private int _dir;
        private float _currentTick;
        private bool _leaving;

        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            LineRenderer = GetComponent<LineRenderer>();
            JesterAnimator = GetComponent<JesterAnimator>();
            JesterFire = GetComponent<JesterFire>();
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

        private void TimestampTick()
        {
            if (_leaving)
            {
                return;
            }

            foreach (T command in jesterCommands)
            {
                if (Mathf.Approximately(enterTimestamp, WaveHandler.Timestamp))
                {
                    MoveIntoView();
                }

                if (Mathf.Approximately(command.GetTimestamp() + enterTimestamp, WaveHandler.Timestamp))
                {
                    OnCommandTime(command);
                }
            }

            if (Mathf.Approximately(LeaveTime, WaveHandler.Timestamp))
            {
                LeaveView();
            }

            if (LineRenderer.enabled)
            {
                LineRenderer.SetPosition(0, transform.position);
            }
        }

        protected abstract void OnCommandTime(T command);

        private void Update()
        {
            if (!Mathf.Approximately(_currentTick, WaveHandler.Timestamp))
            {
                _currentTick = WaveHandler.Timestamp;
                TimestampTick();
            }
        }

        private void MoveIntoView()
        {
            if (_leaving)
            {
                return;
            }

            JesterAnimator.SetMoving();
            transform.DOLocalMoveX(transform.position.x + 2 * _dir, 2);
        }

        private void LeaveView()
        {
            _leaving = true;
            JesterAnimator.SetMoving();
            transform.DOLocalMoveX(transform.position.x + 2 * -_dir, 0.8f);
            AnyJesterDestroyed?.Invoke(gameObject);
            Destroy(gameObject, 1.5f);
        }
    }
}