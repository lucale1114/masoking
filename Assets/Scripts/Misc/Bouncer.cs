using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float bounceFactor = 1.0f; // Determines how much velocity is preserved after the bounce

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Here");
        // Check if the collided object has the Projectile component
        Projectile.DirectProjectile directProjectile = collision.gameObject.GetComponent<Projectile.DirectProjectile>();
        if (directProjectile != null)
        {
            Debug.Log("nowHere");

            // Get the projectile's Rigidbody2D
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("andHere");

                // Reflect the velocity based on the normal of the collision
                Vector2 incomingVelocity = rb.velocity;
                Vector2 normal = collision.contacts[0].normal;
                Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);

                // Apply the bounce factor
                rb.velocity = reflectedVelocity * bounceFactor;
            }
        }
    }
}

