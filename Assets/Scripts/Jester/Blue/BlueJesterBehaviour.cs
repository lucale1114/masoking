using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jester.Blue
{
    public class BlueJesterBehaviour : AbstractStandingJesterBehaviour<BlueJesterCommand>
    {
        [SerializeField] private GameObject _snipeLine;

        private BlueJesterFire _blueJesterFire;

        private new void Start()
        {
            base.Start();
            _blueJesterFire = GetComponent<BlueJesterFire>();
        }

        protected override void CalculateLeaveTime()
        {
            LeaveTime = jesterCommands
                .Select(command =>
                {
                    var time = 0.3f + command.timestamp;
                    if (command.action is BlueJesterActions.FireStorm or BlueJesterActions.FireAimed or BlueJesterActions.Snipe)
                    {
                        time += (command.shotData.amount == 0 ? 1 : command.shotData.amount) * command.shotData.fireBetween;
                    }

                    return time;
                })
                .Prepend(0f)
                .Max() + 0.3f;
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
                    StartCoroutine(FireBurst(data));
                    break;
                case BlueJesterActions.FireCurved:
                    StartCoroutine(FireCurvedShot(data));
                    break;
                case BlueJesterActions.FireWavy:
                    StartCoroutine(FireWavyShot(data));
                    break;
                case BlueJesterActions.FireRow:
                    StartCoroutine(FireRow(data));
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
                yield return new WaitForSeconds(0.25f);
                _blueJesterFire.ShootBasicProjectile(data.speed, data);
                yield return new WaitForSeconds(data.fireBetween - 0.25f);
            }
        }

        // Shots that have gravitation which flips after some time
        private IEnumerator FireCurvedShot(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            yield return new WaitForSeconds(0.25f);
            _blueJesterFire.ShootCurvedShot(data.speed, data.timer, data.gravityDir, 1, data);
        }

        // Shots that use cosine which makes them wavy. Not well implemented and needs changes.
        private IEnumerator FireWavyShot(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            yield return new WaitForSeconds(0.25f);
            _blueJesterFire.ShootWavyShot(data.speed, data.frequency, data.amp, data);
        }

        // Fires a circular row of projectiles. Can be modified with radius and amount of shots.
        private IEnumerator FireRow(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            yield return new WaitForSeconds(0.25f);
            _blueJesterFire.ShootRow(data.speed, data.radius, data.amount, data);
        }

        // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
        private IEnumerator FireBurst(BlueShotDataObject data)
        {
            JesterAnimator.TriggerFire();
            yield return new WaitForSeconds(0.25f);
            _blueJesterFire.ShootBurstShot(data.speed, data.timer, data.amount, data);
        }

        // Fires a storm of shots towards the player.
        private IEnumerator FireStorm(BlueShotDataObject data)
        {
            for (var i = 0; i < data.amount; i++)
            {
                JesterAnimator.TriggerFire();
                yield return new WaitForSeconds(0.25f);
                _blueJesterFire.ShootBasicProjectile(Random.Range(data.speed / 1.5f, data.speed * 1.5f), data);
                yield return new WaitForSeconds(data.fireBetween - 0.25f);
            }
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

            var spriteRenderer =
                Instantiate(_snipeLine, transform.position, Quaternion.identity);
            var lineRenderer = spriteRenderer.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, new Vector3(x, y));
            Destroy(spriteRenderer, data.fireBetween + 1f);

            yield return new WaitForSeconds(data.fireBetween);

            JesterAnimator.TriggerFire();

            yield return new WaitForSeconds(0.25f);
            _blueJesterFire.ShootBasicProjectile(data.speed, data, x, y);
        }
    }
}