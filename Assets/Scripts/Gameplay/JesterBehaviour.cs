using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static WaveData;

public class JesterBehaviour : MonoBehaviour
{

    JesterFire jesterFire;
    int dir;
    float currentTick;
    public JesterCommand[] jesterCommands;


    void Start()
    { 
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
            jesterFire.ShootBasicProjectile(speed, 0);
            yield return new WaitForSeconds(delay);
        }
    }

    // Shots that have gravitation which flips after some time
    public void FireCurvedShot()
    {
        jesterFire.ShootCurvedShot(15, 0.8f, 0.8f, 1);
    }
    // Shots that use cosine which makes them wavy. Not well implemented and needs changes.
    public void FireWavyShot()
    {
        jesterFire.ShootWavyShot(15, 4f, 2);
    }
    // Fires a circular row of projectiles. Can be modified with radius and amount of shots.
    public void FireRow()
    {
        jesterFire.ShootRow(15, 30f, 8);
    }
    // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
    public void FireBurst()
    {
        jesterFire.ShootBurstShot(7.5f, 5f, 128);
    }

    // Fires a storm of shots towards the player.
    IEnumerator FireStorm()
    {
        int amount = Random.Range(30, 40);
        for (int i = 0; i < amount; i++)
        {
            jesterFire.ShootBasicProjectile(Random.Range(10,30)/10, 20);
            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(3);
    }
}
