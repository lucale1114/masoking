using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterFire : MonoBehaviour
{
    private GameObject player;
    public GameObject projectile;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Fires a basic projectile towards the player based on inaccuracy and speed. Set to 0 when using for a perfectly aimed shot.
    public void ShootBasicProjectile(int speed, float inaccuracy)
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0,0, angle + Random.Range(-inaccuracy, inaccuracy))));
        shot.GetComponent<Rigidbody2D>().velocity = -shot.transform.up * speed;
        Destroy(shot, 10);
    }
}
