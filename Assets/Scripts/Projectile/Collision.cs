using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;

namespace Projectile
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;

        private Projectile _projectile;

        private void Start()
        {
           _projectile = GetComponent<Projectile>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var closestPoint = collision.ClosestPoint(transform.position);
                collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(_projectile.data.damage + 5);
                Instantiate(hitVfx, closestPoint, Quaternion.identity);
                SoundManager.PlayHit(closestPoint);
                Destroy(gameObject);
            }
        }
    }
}