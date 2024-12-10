using System;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;
using Wave.Jesters.Red;

namespace Jester
{
    public class RedJesterBehaviour : MonoBehaviour
    {
        public static event Action<GameObject> AnyJesterDestroyed;

        JesterFire jesterFire;
        int dir;
        float currentTick;
        float leaveTime;
        public float enterTimestamp;
        private GameObject player;
        public RedJesterCommand[] jesterCommands;
        private JesterAnimator jesterAnimator;
        public RedShotDataObject shotDataObject;
        private LineRenderer lineRenderer;
        private bool leaving;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            lineRenderer = GetComponent<LineRenderer>();
            jesterAnimator = GetComponent<JesterAnimator>();
            jesterFire = GetComponent<JesterFire>();
            if (transform.position.x < 0)
            {
                FlipDirection();
                dir = 1;
            }
            else
            {
                dir = -1;
            }

            foreach (RedJesterCommand command in jesterCommands)
            {
                RedShotDataObject data = command.shotData;
                int additionIfOnlyFB = 0;
                if (data.amount == 0)
                {
                    additionIfOnlyFB++;
                }
                leaveTime = Mathf.Max(enterTimestamp + command.timestamp + 1f, (enterTimestamp + command.timestamp + ((data.amount + additionIfOnlyFB) * data.fireBetween) + 0.5f));
            }
        }

        private void FlipDirection()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        private void TimestampTick()
        {
            if (leaving) {
                return;
            }
            foreach (var command in jesterCommands)
            {
                if (Mathf.Approximately(enterTimestamp, WaveHandler.Timestamp))
                {
                    MoveIntoView();
                }

                if (Mathf.Approximately(command.timestamp + enterTimestamp, WaveHandler.Timestamp))
                {
                    PerformAction(command.action, command.shotData);
                }
            }
            if (Mathf.Approximately(leaveTime, WaveHandler.Timestamp))
            {
                LeaveView();
            }
            if (lineRenderer.enabled)
            {
                lineRenderer.SetPosition(0, transform.position);
            }
        }

        private void Update()
        {
            if (!Mathf.Approximately(currentTick, WaveHandler.Timestamp)) {
                currentTick = WaveHandler.Timestamp;
                TimestampTick();
            }
        }

        private void PerformAction(RedJesterActions action, RedShotDataObject data)
        {
            jesterAnimator.SetIdle();

            switch (action)
            {
                case RedJesterActions.Throw:
                    Throw(data);
                    break;
                case RedJesterActions.ThrowAndRoll:
                    ThrowAndRoll(data);
                    break;
            }
        }

        private void MoveIntoView()
        {
            if (leaving)
            {
                return;
            }
            jesterAnimator.SetMoving();
            transform.DOLocalMoveX(transform.position.x + 2 * dir, 2);
        }
        private void LeaveView()
        {
            leaving = true;
            jesterAnimator.SetMoving();
            transform.DOLocalMoveX(transform.position.x + 2 * -dir, 0.8f);
            AnyJesterDestroyed?.Invoke(gameObject);
            Destroy(gameObject, 1.5f);
        }

        private void Throw(RedShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.Throw(data);
        }

        private void ThrowAndRoll(RedShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ThrowAndRoll(data);
        }

    }
}
