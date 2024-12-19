using Jester.Green;
using Player;
using Projectile;
using UnityEngine;

namespace Misc
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private GameObject _wallDashAnimationPrefab;
        [SerializeField] private AudioClip[] AudioClip;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Projectile"))
            {
                var projectile = collision.collider.GetComponent<IProjectile>();

                if (projectile.GetNumberOfBounces() > 0)
                {

                    projectile.AttemptBounce(-collision.contacts[0].normal);
                }
            }

            var movement = collision.collider.GetComponent<Player.Movement>();

            if (collision.collider.CompareTag("Player") && !movement.IsCurrentlyDashing)
            {
                movement = collision.collider.GetComponent<Player.Movement>();
                movement.currentVelocity.x = Vector2.zero.x;
            }
        }


        private void OnCollisionStay2D(Collision2D collision)
        {
            var movement = collision.collider.GetComponent<Player.Movement>();
            if (collision.collider.CompareTag("Player") && movement.IsCurrentlyDashing )
            {
                movement = collision.collider.GetComponent<Player.Movement>();
                movement.AttemptBounce(-collision.contacts[0].normal);
                SoundFXManager.Instance.PlayRandomSoundFXNoPitch(AudioClip,1f);

                Instantiate(_wallDashAnimationPrefab,
                    transform.GetComponent<Collider2D>().ClosestPoint(collision.transform.position),
                    Quaternion.identity);
            }
        }
    }
}