using UnityEngine;

public class CollisionProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(5);
            Destroy(gameObject);
        }
    }
}