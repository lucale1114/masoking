using Projectile;
using UnityEngine;

namespace Misc
{
    public class PlayArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                other.gameObject.GetComponent<IProjectile>().SetActive();
            }
        }
    }
}