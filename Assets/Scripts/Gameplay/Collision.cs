using Player;
using UnityEngine;

namespace Gameplay
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private GameObject hitVfx;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(5);
                Instantiate(hitVfx, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}