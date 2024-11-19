using Managers;
using Player;
using UnityEngine;

namespace Projectile
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;
        Projectile projectileScript;

        private void Start()
        {
           projectileScript = GetComponent<Projectile>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(projectileScript.data.damage + 5);
                Instantiate(hitVfx, transform.position, Quaternion.identity);
                SoundManager.PlayHit(transform.position);
                Destroy(gameObject);
            }
        }
    }
}