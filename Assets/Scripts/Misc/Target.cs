using UnityEngine;

namespace Misc
{
    public class Target : MonoBehaviour
    {
        public GameObject targetProjectile;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (targetProjectile == other.gameObject)
            {
                other.gameObject.GetComponent<Projectile.Collision>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}
