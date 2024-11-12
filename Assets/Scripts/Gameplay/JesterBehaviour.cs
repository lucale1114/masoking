using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterBehaviour : MonoBehaviour
{
    JesterFire jesterFire;

    void Start()
    { 
        jesterFire = GetComponent<JesterFire>();
        StartCoroutine(FireStorm());
    }

    // Update is called once per frame
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

    IEnumerator FireStorm()
    {
        int amount = Random.Range(30, 40);
        for (int i = 0; i < amount; i++)
        {
            jesterFire.ShootBasicProjectile(Random.Range(10,30)/10, 20);
            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(FireAimedShots());
    }
}
