using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            other.gameObject.GetComponent<Projectile.ProjectileCollision>().enabled = true;
            Destroy(gameObject);
        }
    }
}
