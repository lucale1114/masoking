using Projectile;
using UnityEngine;

namespace Misc
{
    public class Wall : MonoBehaviour
    {
        public Vector2 normal;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Projectile"))
            {
                var projectile = collision.collider.GetComponent<IProjectile>();

                if (projectile.GetNumberOfBounces() > 0)
                {
                    projectile.AttemptBounce(normal);
                }
            }
        }
    }
}