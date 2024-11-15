using Player;
using UnityEngine;

public class CollisionProjectile : MonoBehaviour
{
    public Movement Movement;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HeatSystem>().ChangeHeat(5);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Movement.currentVelocity = Vector2.zero;
        }
    }
}