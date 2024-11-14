using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterFire : MonoBehaviour
{
    private GameObject player;
    public GameObject projectile;
    // A shoot function for the jesters that can be called whenever.
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
  
    // Fires a basic projectile towards the player based on inaccuracy and speed. Set to 0 when using for a perfectly aimed shot.
    public Projectile ShootBasicProjectile(float speed, float inaccuracy)
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0,0, angle + Random.Range(-inaccuracy, inaccuracy))));
        shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
        Destroy(shot, 10);
        return shot.GetComponent<Projectile>();
    }

    public void ShootBurstShot(float speed, float time, int burst)
    {
        Projectile shot = ShootBasicProjectile(speed, 0);
        shot.speed = speed;
        shot.burstTimer = time;
        shot.burst = burst;
    }

    public void ShootRow(float speed, float radius, int amount)
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        for (float i = angle - radius; i < angle + radius; i += (radius/amount) * 2)
        {
            print(i);
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, i)));
            shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
        }
    }
    public void ShootWavyShot(float speed, float frequency, int amp)
    {
        Projectile shot = ShootBasicProjectile(speed, 0);
        shot.spin = true;
        shot.frequency = frequency;
        shot.amp = amp;
        shot.speed = speed;
    }

    public void ShootCurvedShot(float speed, float time, float dir, int wave)
    {
        Projectile shot = ShootBasicProjectile(speed, 0);
        shot.spin = true;
        shot.gravityTimer = time;
        shot.gravityDir = dir;
        shot.flipAmount = wave;
    }
}
