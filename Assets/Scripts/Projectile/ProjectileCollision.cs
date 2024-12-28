using Player;
using UnityEngine;
using static Player.HeatSystem.HeatSource;

namespace Projectile
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private AudioClip[] SoundFX;

        private IProjectile _projectile;
        public bool noStabbing = false;

        private void Start()
        {
            _projectile = GetComponent<IProjectile>() ?? GetComponentInParent<IProjectile>();
        }

        private void ReflectDebounceEnable()
        {
            GetComponent<Collider2D>().enabled = true;
        }
        private void CatchProjectile(Collider2D collision)
        {
            var damage = _projectile.GetShotData().GetDamage() * _projectile.GetDamageMod();
            var closestPoint = collision.ClosestPoint(transform.position);

            var direction = closestPoint - (Vector2) transform.position;

            var heatSystem = collision.gameObject.GetComponent<HeatSystem>();

            if (gameObject.GetComponentInParent<AbstractThrownProjectile>())
            {
                heatSystem.ChangeHeat(damage, Blunt, closestPoint);
            }
            else
            {
                heatSystem.ChangeHeat(damage,Sharp, closestPoint, direction);
            }

            //SoundFXManager.Instance.PlayRandomSoundFX(SoundFX, 1f);
            //SoundFXManager.Instance.PitchChange();
            SoundFXManager20.Instance.PlaySoundFX(SoundType.Slash,0.8f);

            var topLevelItem = gameObject.transform;
            while (topLevelItem.parent != null)
            {
                topLevelItem = topLevelItem.parent;
            }
            Destroy(topLevelItem.gameObject);
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
                    if (_projectile.GetShotData().GetBreakable())
                    {
                        if (collision.GetComponent<Movement>().IsCurrentlyDashing)
                        {
                            CatchProjectile(collision);
                            return;
                        }
                        float speed = gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
                        ContactPoint2D[] contacts = new ContactPoint2D[2];
                        collision.GetContacts(contacts);
                        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity.normalized, contacts[0].normal) * -speed;
                        GetComponent<Collider2D>().enabled = false;
                        Invoke(nameof(ReflectDebounceEnable), 0.15f);
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