using System.Collections;
using DG.Tweening;
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
            bool foundEnter = false;
            bool foundLeave = false;
            foreach (JesterCommand command in jesterCommands)
            {
                if (command.action == Actions.Enter) {
                    foundEnter = true;
                } 
                if (command.action == Actions.Leave)
                {
                    foundLeave = true;
                }
            }
            if (!foundEnter)
            {
                Invoke("ForceEnter", 0.01f);
            }
            if (!foundLeave)
            {
                foreach (JesterCommand command in jesterCommands)
                {
                    ShotDataObject data = command.shotData;
                    int additionIfOnlyFB = 0;
                    if (data.amount == 0)
                    {
                        additionIfOnlyFB++;
                    }
                    leaveTime = (timestampEntered + (command.timestamp - timestampEntered)) + (data.amount + additionIfOnlyFB * data.fireBetween) + 1f;
                }
            }
        }
        void ForceEnter()
        {
            PerformAction(Actions.Enter, new ShotDataObject());
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
                if (Mathf.Approximately(command.timestamp, Timestamp))
                {
                    PerformAction(command.action, command.shotData);
                }
            }
            if (Mathf.Approximately(leaveTime, Timestamp))
            {
                PerformAction(Actions.Leave, new ShotDataObject());
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
            if (action is Actions.Enter or Actions.Leave)
            {
                jesterAnimator.SetMoving();
            }
            else
            {
                jesterAnimator.SetIdle();
            }

            switch (action)
            {
                case Actions.Enter:
                    MoveIntoPlayfield();
                    break;
                case Actions.Leave:
                    LeavePlayfield();
                    break;
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
            }
        }

        void MoveIntoPlayfield()
        {
            transform.DOLocalMoveX(transform.position.x + 2 * dir, 2);
        }
        void LeavePlayfield()
        {
            transform.DOLocalMoveX(transform.position.x + 2 * -dir, 0.8f);
            Destroy(gameObject, 2);
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
            float x = data.advancedSettings.x;
            float y = data.advancedSettings.y;
            if (x == 0 && y == 0)
            {
                x = player.transform.position.x;
                y = player.transform.position.y;
            }
            if (data.advancedSettings.randomY)
            {
                y = Random.Range(-40, 40) / 10;
            }
            if (data.advancedSettings.randomX)
            {
                x = Random.Range(-50, 50) / 10;
            }
            GameObject target = Instantiate(Resources.Load($"Misc/Target") as GameObject, new Vector3(x, y), transform.rotation);
            yield return new WaitForSeconds(data.fireBetween);
            jesterAnimator.TriggerFire();
            jesterFire.Snipe(data, x, y, target);
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
    }
}
