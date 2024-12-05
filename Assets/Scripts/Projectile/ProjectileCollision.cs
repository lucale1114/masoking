using Player;
using UnityEngine;

namespace Projectile
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;
        [SerializeField] private AudioClip[] SoundFX;


        private IProjectile _projectile;
        public bool noStabbing = false;

        private void Start()
        {
            _projectile = GetComponent<IProjectile>();
        }

        private void CatchProjectile(Collider2D collision)
        {
            var damage = _projectile.GetShotData().damage * _projectile.GetDamageMod();
            var closestPoint = collision.ClosestPoint(transform.position);
            collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(damage);
            Instantiate(hitVfx, closestPoint, Quaternion.identity);
            SoundFXManager.Instance.PlayRandomSoundFX(SoundFX, transform, 1f);
            SoundFXManager.Instance.PitchChange();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_projectile.CanHitThings())
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (!noStabbing)
                    {
                        CatchProjectile(collision);
                        return;
                    }
                    if (_projectile.GetShotData().breakable)
                    {
                        if (collision.GetComponent<Movement>().IsCurrentlyDashing)
                        {
                            CatchProjectile(collision);
                            return;
                        }
                        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, collision.GetComponent<Rigidbody2D>().velocity) * -0.5f;
                        return;
                    }
                    gameObject.GetComponent<Rigidbody2D>().velocity *= -0.5f;
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    //gameObject.AddComponent<PolygonCollider2D>();
                }
            }
        }
    }
}