using System.Collections;
using DG.Tweening;
using Misc;
using Unity.VisualScripting;
using UnityEngine;
using static WaveData;

namespace Jester
{
    public class JesterBehaviour : MonoBehaviour
    {

        JesterFire jesterFire;
        int dir;
        float currentTick;
        float leaveTime;
        public float enterTimestamp;
        private GameObject player;
        public JesterCommand[] jesterCommands;
        private JesterAnimator jesterAnimator;
        public ShotDataObject shotDataObject;


        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            jesterAnimator = GetComponent<JesterAnimator>();
            jesterFire = GetComponent<JesterFire>();
            if (transform.position.x < 0)
            {
                dir = 1;
            }
            else
            {
                FlipDirection();
                dir = -1;
            }
            float timestampEntered = Timestamp;

            Invoke("ForceEnter", 0.01f);
            foreach (JesterCommand command in jesterCommands)
            {
                ShotDataObject data = command.shotData;
                int additionIfOnlyFB = 0;
                if (data.amount == 0)
                {
                    additionIfOnlyFB++;
                }
                leaveTime = (timestampEntered + command.timestamp + ((data.amount + additionIfOnlyFB) * data.fireBetween) + 1f);
            }
        }
        void ForceEnter()
        {
            jesterAnimator.SetMoving();
            MoveIntoPlayfield();
        }

        private void FlipDirection()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        public void TimestampTick()
        {
            foreach (JesterCommand command in jesterCommands)
            {
                if (Mathf.Approximately(command.timestamp + enterTimestamp, Timestamp))
                {
                    PerformAction(command.action, command.shotData);
                }
            }
            if (Mathf.Approximately(leaveTime, Timestamp))
            {
                jesterAnimator.SetMoving();
                LeavePlayfield();
            }
        }

        void Update()
        {
            if (currentTick != Timestamp) {
                currentTick = Timestamp;
                TimestampTick();
            }
        }

        void PerformAction(Actions action, ShotDataObject data)
        {
            jesterAnimator.SetIdle();

            switch (action)
            {
                case Actions.FireAimed:
                    StartCoroutine(FireAimedShots(data));
                    break;
                case Actions.FireStorm:
                    StartCoroutine(FireStorm(data));
                    break;
                case Actions.FireBurst:
                    FireBurst(data);
                    break;
                case Actions.FireCurved:
                    FireCurvedShot(data);
                    break;
                case Actions.FireWavy:
                    FireWavyShot(data);
                    break;
                case Actions.FireRow:
                    FireRow(data);
                    break;
                case Actions.FireSniper:
                    StartCoroutine(FireSniper(data));
                    break;
                case Actions.Throw:
                    Throw(data);
                    break;
            }
        }

        private void MoveIntoPlayfield()
        {
            transform.DOLocalMoveX(transform.position.x + 2 * dir, 2);
        }
        private void LeavePlayfield()
        {
            transform.DOLocalMoveX(transform.position.x + 2 * -dir, 0.8f);
            Destroy(gameObject, 1.5f);
        }
        // Shots that are aimed towards the player
        IEnumerator FireAimedShots(ShotDataObject data)
        {
            for (int i = 0; i < data.amount; i++) {
                jesterAnimator.TriggerFire();
                jesterFire.ShootBasicProjectile(data.speed, data);
                yield return new WaitForSeconds(data.fireBetween);
            }
        }

        // Shots that have gravitation which flips after some time
        private void FireCurvedShot(ShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootCurvedShot(data.speed, data.timer, data.gravityDir, 1, data);
        }
        // Shots that use cosine which makes them wavy. Not well implemented and needs changes.
        private void FireWavyShot(ShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootWavyShot(data.speed, data.frequency, data.amp, data);
        }
        // Fires a circular row of projectiles. Can be modified with radius and amount of shots.
        private void FireRow(ShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootRow(data.speed, data.radius, data.amount, data);
        }
        // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
        private void FireBurst(ShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.ShootBurstShot(data.speed, data.timer, data.amount, data);
        }
        private IEnumerator FireSniper(ShotDataObject data)
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
            GameObject target = Instantiate(Resources.Load($"Misc/Target") as GameObject, new Vector3(x, y), transform.rotation);
            yield return new WaitForSeconds(data.fireBetween);
            jesterAnimator.TriggerFire();
            target.GetComponent<Target>().targetProjectile = jesterFire.Snipe(data, x, y, target);
        }

        // Fires a storm of shots towards the player.
        IEnumerator FireStorm(ShotDataObject data)
        {
            for (int i = 0; i < data.amount; i++)
            {
                jesterFire.ShootBasicProjectile(Random.Range(data.speed / 1.5f, data.speed * 1.5f), data);
                yield return new WaitForSeconds(data.fireBetween);
            }
            yield return new WaitForSeconds(3);
        }

        private void Throw(ShotDataObject data)
        {
            jesterAnimator.TriggerFire();
            jesterFire.Throw(data);
        }
    }
}
