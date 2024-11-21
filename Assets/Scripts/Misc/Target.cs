using UnityEngine;

namespace Misc
{
    public class Target : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile"))
            {
                other.gameObject.GetComponent<Projectile.Collision>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}
