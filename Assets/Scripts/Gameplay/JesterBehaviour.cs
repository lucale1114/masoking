using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterBehaviour : MonoBehaviour
{
    JesterFire jesterFire;
    int dir;
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
        StartCoroutine(tempMove());
    }

    // Temporary function. Planned to be replaced by tweening
    IEnumerator tempMove()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            transform.position += new Vector3(0.15f, 0, 0) * dir;
        }
        yield return new WaitForSeconds(2);
        FireBurst();
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            transform.position -= new Vector3(0.5f, 0, 0) * dir;
        }
    }

    // Shots that are aimed towards the player
    IEnumerator FireAimedShots()
    {
        int amount = Random.Range(3, 5);
        for (int i = 0; i < amount; i++) {
            jesterFire.ShootBasicProjectile(5, 0);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(FireAimedShots());
    }

    // Shots that have gravitation which flips after some time
    public void FireCurvedShot()
    {
        jesterFire.ShootCurvedShot(5, 0.8f, -0.4f, 1);
    }

    // Shots that have gravitation which gives them wavy movement. Not well implemented and probably needs to be redone
    public void FireWavyShot()
    {
        jesterFire.ShootCurvedShot(5, 0.8f, 1.2f, 99);
    }

    // Fires a burst shot which explodes into the amount of shots given in the 3rd argument
    public void FireBurst()
    {
        jesterFire.ShootBurstShot(2.5f, 5f, 16);
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
