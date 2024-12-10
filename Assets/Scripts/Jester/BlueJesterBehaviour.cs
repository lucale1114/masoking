using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;
using Wave.Jesters.Blue;
using Random = UnityEngine.Random;

namespace Jester
{
    public class BlueJesterBehaviour : MonoBehaviour
    {
        public static event Action<GameObject> AnyJesterDestroyed;

        JesterFire jesterFire;
        int dir;
        float currentTick;
        float leaveTime;
        public float enterTimestamp;
        private GameObject player;
        public BlueJesterCommand[] jesterCommands;
        private JesterAnimator jesterAnimator;
        public BlueShotDataObject shotDataObject;
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

            foreach (BlueJesterCommand command in jesterCommands)
            {
                BlueShotDataObject data = command.shotData;
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
            foreach (BlueJesterCommand command in jesterCommands)
            {
                if (Mathf.Approximately(enterTimestamp, WaveHandler.Timestamp))
                {
                    MoveIntoView();
                }

                if (Mathf.Approximately(command.timestamp + enterTimestamp, WaveHandler.Timestamp))
                {
                    PerformAction(command.jesterAction, command.shotData);
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

        private void PerformAction(BlueJesterActions action, BlueShotDataObject data)
        {
            jesterAnimator.SetIdle();

            switch (action)
            {
                case BlueJesterActions.FireAimed:
                    StartCoroutine(FireAimedShots(data));
                    break;
                case BlueJesterActions.FireStorm:
                    StartCoroutine(FireStorm(data));
                    break;
                case BlueJesterActions.FireBurst:
                    FireBurst(data);
                    break;
                case BlueJesterActions.FireCurved:
                    FireCurvedShot(data);
                    break;
                case BlueJesterActions.FireWavy:
                    FireWavyShot(data);
                    break;
                case BlueJesterActions.FireRow:
                    FireRow(data);
                    break;
                case BlueJesterActions.Snipe:
                    StartCoroutine(FireSniper(data));
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
        // Shots that are aimed towards the player
        IEnumerator FireAimedShots(BlueShotDataObject data)
        {
            for (int i = 0; i < data.amount; i++) {
                jesterAnimator.TriggerFire();
                jesterFire.ShootBasicProjectile(data.speed, data);
                yield return new WaitForSeconds(data.fireBetween);
            }
        }

        // Shots that have gravitation which flips after some time
        private void FireCurvedShot(BlueShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootCurvedShot(data.speed, data.timer, data.gravityDir, 1, data);
        }
        // Shots that use cosine which makes them wavy. Not well implemented and needs changes.
        private void FireWavyShot(BlueShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootWavyShot(data.speed, data.frequency, data.amp, data);
        }
        // Fires a circular row of projectiles. Can be modified with radius and amount of shots.
        private void FireRow(BlueShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootRow(data.speed, data.radius, data.amount, data);
        }
        // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
        private void FireBurst(BlueShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootBurstShot(data.speed, data.timer, data.amount, data);
        }

        // Fires a storm of shots towards the player.
        IEnumerator FireStorm(BlueShotDataObject data)
        {
            for (int i = 0; i < data.amount; i++)
            {
                jesterFire.ShootBasicProjectile(Random.Range(data.speed / 1.5f, data.speed * 1.5f), data);
                yield return new WaitForSeconds(data.fireBetween);
            }
            yield return new WaitForSeconds(3);
        }

        IEnumerator FireSniper(BlueShotDataObject data)
        {
            float x = data.x;
            float y = data.y;
            if (x == 0 && y == 0)
            {
                x = player.transform.position.x;
                y = player.transform.position.y;
            }
            if (data.randomY)
            {
                y = Random.Range(-4.0f, 4.0f);
            }
            if (data.randomX)
            {
                x = Random.Range(-5.0f, 4.0f);
            }
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, new Vector3(x,y));

            yield return new WaitForSeconds(data.fireBetween);
            jesterAnimator.TriggerFire();
            jesterFire.ShootBasicProjectile(data.speed, data, x, y);
            yield return new WaitForSeconds(0.25f);
            lineRenderer.enabled = false;

        }
    }
}
