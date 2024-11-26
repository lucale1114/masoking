using Misc;
using Player;
using UnityEngine;

namespace Projectile
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;
        [SerializeField] private AudioClip[] Slashes;

        private IProjectile _projectile;
        public bool noStabbing = false;

        private void Start()
        {
            _projectile = GetComponent<IProjectile>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_projectile.CanHitThings())
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (!noStabbing)
                    {
                        var damage = (5 + _projectile.GetShotData().damage) * _projectile.GetDamageMod();
                        var closestPoint = collision.ClosestPoint(transform.position);
                        collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(damage);
                        Instantiate(hitVfx, closestPoint, Quaternion.identity);
                        //SoundManager.PlayHit(closestPoint);
                        SoundFXManager.Instance.PlayRandomSoundFX(Slashes, transform, 1f);
                        Destroy(gameObject);
                        return;
                    }

                    gameObject.GetComponent<Rigidbody2D>().velocity *= -0.5f;
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    gameObject.AddComponent<PolygonCollider2D>();
                }

                if (collision.gameObject.CompareTag("Wall"))
                {
                    if (_projectile.GetNumberOfBounces() > 0)
                    {
                        _projectile.Bounce(collision.gameObject.GetComponent<Wall>().normal);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}