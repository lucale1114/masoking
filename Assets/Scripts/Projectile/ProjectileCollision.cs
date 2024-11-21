using Managers;
using Player;
using UnityEngine;

namespace Projectile
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;

        private IProjectile _projectile;

        private void Start()
        {
            _projectile = GetComponent<IProjectile>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_projectile != null)
                {
                    var damage = (5 + _projectile.GetShotData().damage) * _projectile.GetDamageMod();
                    var closestPoint = collision.ClosestPoint(transform.position);
                    collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(damage);
                    //Instantiate(hitVfx, closestPoint, Quaternion.identity);
                    SoundManager.PlayHit(closestPoint);
                    Destroy(gameObject);
                }
            }
        }
    }
}