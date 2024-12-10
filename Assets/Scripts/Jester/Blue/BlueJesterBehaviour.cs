using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jester.Blue
{
    public class BlueJesterBehaviour : AbstractStandingJesterBehaviour<BlueJesterCommand>
    {
        protected override void CalculateLeaveTime()
        {
            foreach (var command in jesterCommands)
            {
                var data = command.shotData;
                var additionIfOnlyFb = 0;
                if (data.amount == 0)
                {
                    additionIfOnlyFb++;
                }

                LeaveTime = Mathf.Max(enterTimestamp + command.timestamp + 1f,
                    enterTimestamp + command.timestamp + (data.amount + additionIfOnlyFb) * data.fireBetween + 0.5f);
            }
        }

        protected override void OnCommandTime(BlueJesterCommand command)
        {
            JesterAnimator.SetIdle();

            var data = command.shotData;

            switch (command.action)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Shots that are aimed towards the player
        private IEnumerator FireAimedShots(BlueShotDataObject data)
        {
            for (var i = 0; i < data.amount; i++)
            {
                JesterAnimator.TriggerFire();
                JesterFire.ShootBasicProjectile(data.speed, data);
                yield return new WaitForSeconds(data.fireBetween);
            }
        }

        // Shots that have gravitation which flips after some time
        private void FireCurvedShot(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            JesterFire.ShootCurvedShot(data.speed, data.timer, data.gravityDir, 1, data);
        }

        // Shots that use cosine which makes them wavy. Not well implemented and needs changes.
        private void FireWavyShot(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            JesterFire.ShootWavyShot(data.speed, data.frequency, data.amp, data);
        }

        // Fires a circular row of projectiles. Can be modified with radius and amount of shots.
        private void FireRow(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            JesterFire.ShootRow(data.speed, data.radius, data.amount, data);
        }

        // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
        private void FireBurst(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            JesterFire.ShootBurstShot(data.speed, data.timer, data.amount, data);
        }

        // Fires a storm of shots towards the player.
        private IEnumerator FireStorm(BlueShotDataObject data)
        {
            for (var i = 0; i < data.amount; i++)
            {
                JesterFire.ShootBasicProjectile(Random.Range(data.speed / 1.5f, data.speed * 1.5f), data);
                yield return new WaitForSeconds(data.fireBetween);
            }

            yield return new WaitForSeconds(3);
        }

        private IEnumerator FireSniper(BlueShotDataObject data)
        {
            var x = data.x;
            var y = data.y;
            if (x == 0 && y == 0)
            {
                x = Player.transform.position.x;
                y = Player.transform.position.y;
            }

            if (data.randomY)
            {
                y = Random.Range(-4.0f, 4.0f);
            }

            if (data.randomX)
            {
                x = Random.Range(-5.0f, 4.0f);
            }

            LineRenderer.enabled = true;
            LineRenderer.SetPosition(1, new Vector3(x, y));

            yield return new WaitForSeconds(data.fireBetween);
            JesterAnimator.TriggerFire();
            JesterFire.ShootBasicProjectile(data.speed, data, x, y);
            yield return new WaitForSeconds(0.25f);
            LineRenderer.enabled = false;
        }
    }
}