using Managers;
using Player;
using UnityEngine;

namespace Projectile
{
    public class ProjectileCollision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;

        private Projectile _projectile;
        private ThrowProjectile _throwProjectile;

        private void Start()
        {
            _projectile = GetComponent<Projectile>();
            _throwProjectile = GetComponent<ThrowProjectile>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!enabled)
            {
                return;
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                var damage = 5 + (_projectile ? _projectile.data.damage : _throwProjectile.data.damage);
                var closestPoint = collision.ClosestPoint(transform.position);
                collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(damage);
                Instantiate(hitVfx, closestPoint, Quaternion.identity);
                SoundManager.PlayHit(closestPoint);
                Destroy(gameObject);
            }
        }
    }
}