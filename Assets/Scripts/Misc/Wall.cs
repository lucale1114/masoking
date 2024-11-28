using Player;
using Projectile;
using UnityEngine;

namespace Misc
{
    public class Wall : MonoBehaviour
    {
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

            if (collision.collider.CompareTag("Player"))
            {
                var movement = collision.collider.GetComponent<Movement>();
                movement.AttemptBounce(-collision.contacts[0].normal);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                var movement = collision.collider.GetComponent<Movement>();
                movement.AttemptBounce(-collision.contacts[0].normal);
            }
        }
    }
}