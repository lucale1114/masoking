using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gameplay;
using static WaveData;

public class JesterBehaviour : MonoBehaviour
{

    JesterFire jesterFire;
    int dir;
    float currentTick;
    public JesterCommand[] jesterCommands;

    private JesterAnimator jesterAnimator;

    void Start()
    {
        jesterAnimator = GetComponent<JesterAnimator>();
        jesterFire = GetComponent<JesterFire>();
        if (transform.position.x < 0)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
    }

    public void TimestampTick()
    {
        foreach (JesterCommand command in jesterCommands)
        {
            if (Mathf.Approximately(command.timestamp, Timestamp))
            {
                print(command.action);
                PerformAction(command.action, command.shotData);
            }
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
                StartCoroutine(FireAimedShots(data.speed, data.amount, data.fireBetween));
                break;
            case Actions.FireStorm:
                StartCoroutine(FireStorm(data.speed, data.amount, data.inaccuracy, data.fireBetween));
                break;
            case Actions.FireBurst:
                FireBurst(data.speed, data.timer, data.amount);
                break;
            case Actions.FireCurved:
                FireCurvedShot(data.speed, data.timer, data.gravityDir);
                break;
            case Actions.FireWavy:
                FireWavyShot(data.speed, data.frequency, data.amp);
                break;
            case Actions.FireRow:
                FireRow(data.speed, data.radius, data.amount);
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
    }
    // Shots that are aimed towards the player
    IEnumerator FireAimedShots(float speed, int amount, float delay)
    {
        for (int i = 0; i < amount; i++) {
            jesterAnimator.TriggerFire();
            jesterFire.ShootBasicProjectile(speed, 0);
            yield return new WaitForSeconds(delay);
        }
    }

    // Shots that have gravitation which flips after some time
    public void FireCurvedShot(float speed, float time, float dir)
    {
        jesterAnimator.TriggerFire();
        jesterFire.ShootCurvedShot(speed, time, dir, 1);
    }
    // Shots that use cosine which makes them wavy. Not well implemented and needs changes.
    public void FireWavyShot(float speed, float frequency, int amp)
    {
        jesterAnimator.TriggerFire();
        jesterFire.ShootWavyShot(speed, frequency, amp);
    }
    // Fires a circular row of projectiles. Can be modified with radius and amount of shots.
    public void FireRow(float speed, float radius, int amount)
    {
        jesterAnimator.TriggerFire();
        jesterFire.ShootRow(speed, radius, amount);
    }
    // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
    public void FireBurst(float speed, float time, int amount)
    {
        jesterAnimator.TriggerFire();
        jesterFire.ShootBurstShot(speed, time, amount);
    }

    // Fires a storm of shots towards the player.
    IEnumerator FireStorm(float speed, int amount, int inaccuracy, float time)
    {
        for (int i = 0; i < amount; i++)
        {
            jesterFire.ShootBasicProjectile(Random.Range(speed / 1.5f, speed * 1.5f), inaccuracy);
            yield return new WaitForSeconds(time);
        }
        yield return new WaitForSeconds(3);
    }
}
