using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveData;
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
    public Projectile ShootBasicProjectile(float speed, float inaccuracy, ShotDataObject data)
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0,0, angle + Random.Range(-inaccuracy, inaccuracy))));
        shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
        Projectile projectileScript = shot.GetComponent<Projectile>();
        projectileScript.data = data;
        Destroy(shot, 10);
        return projectileScript;
    }

    public void ShootBurstShot(float speed, float time, int burst, ShotDataObject data)
    {
        Projectile shot = ShootBasicProjectile(speed, 0, data);
        shot.speed = speed;
        shot.burstTimer = time;
        shot.burst = burst;
    }

    public void ShootRow(float speed, float radius, int amount, ShotDataObject data)
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
    public void ShootWavyShot(float speed, float frequency, int amp, ShotDataObject data)
    {
        Projectile shot = ShootBasicProjectile(speed, 0, data);
        shot.spin = true;
        shot.frequency = frequency;
        shot.amp = amp;
        shot.speed = speed;
    }

    public void ShootCurvedShot(float speed, float time, float dir, int wave, ShotDataObject data)
    {
        Projectile shot = ShootBasicProjectile(speed, 0, data);
        shot.spin = true;
        shot.gravityTimer = time;
        shot.gravityDir = dir;
        shot.flipAmount = wave;
    }
}
