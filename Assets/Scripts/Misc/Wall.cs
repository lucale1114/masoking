using Projectile;
using UnityEngine;

namespace Misc
{
    public class Wall : MonoBehaviour
    {
        public Vector2 normal;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile"))
            {
                var projectile = other.GetComponent<IProjectile>();

                if (projectile.GetNumberOfBounces() > 0)
                {
                    projectile.AttemptBounce(normal);
                }
            }
        }
    }
}