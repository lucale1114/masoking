using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectileRef;

    private float spinSpeed;

    public int burst;
    public float burstTimer;
    public float speed;
    public bool spin;

    void Start()
    {
        if ( burstTimer > 0 )
        {
            StartCoroutine(Burst());
        }
        if (spin)
        {
            spinSpeed = Random.Range(6.0f, 7.0f) * (Random.Range(0, 2) * 2 - 1);
            InvokeRepeating("Spin", 0, 0.005f);
        }
    }

    void Spin()
    {
        transform.rotation *= Quaternion.Euler(0, 0, spinSpeed);
    }

    IEnumerator Burst()
    {
        yield return new WaitForSeconds(burstTimer);
        float angle = 360 / burst;
        print(angle);
        for (float i = 0; i < 360; i += angle)
        {
            GameObject shot = Instantiate(projectileRef, transform.position, Quaternion.Euler(0, 0, i));
            print(-shot.transform.up * speed);
            shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
            shot.GetComponent<Projectile>().burstTimer = 0;
            shot.GetComponent<Projectile>().spin = true;
            Destroy(shot, 10);
        }
        Destroy(gameObject);
    }
}
