using Misc;
using Player;
using Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectCollision : MonoBehaviour
{

    [SerializeField] private Movement movement;
    [SerializeField] private IntroUserInterface intro;
    [SerializeField] private LineRenderer lineRenderer;


    bool hasDashed = false;

    public bool HasDashed => hasDashed;


    private void Awake()
    {

         lineRenderer.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && movement.IsCurrentlyDashing)
        {
            StartCoroutine(FallOver(collision.gameObject, movement.transform.position));

        }
    }

    private IEnumerator FallOver(GameObject gameObject, Vector2 playerPosition)
    {

        hasDashed = true;


        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (hasDashed != false)
            {


                // Temporarily disable Rigidbody2D to manually animate the fall
                rb.isKinematic = true;

                // Calculate the fall direction based on player position
                Vector2 impactDirection = (gameObject.transform.position - (Vector3)playerPosition).normalized;
                float fallAngle = impactDirection.x > 0 ? -90f : 90f; // Right or left fall
                float rotationTime = 0.8f; // Time it takes to fall
                Quaternion startRotation = gameObject.transform.rotation;
                Quaternion endRotation = Quaternion.Euler(0, 0, fallAngle);

                float wiggleAngle = 7;
                float wiggleCount = 6f;
                float wiggleDuration = 0.25f;

                lineRenderer.enabled = true;
                lineRenderer.useWorldSpace = true;

                if (impactDirection.x > 0) // Dash from left
                {
                    Debug.Log("Got here");
                    lineRenderer.SetPosition(0, transform.position + transform.right * 5f); // Extend line to the left
                    lineRenderer.SetPosition(1, transform.position);
                }
                else // Dash from right
                {
                    Debug.Log("Got here now");
                    lineRenderer.SetPosition(0, transform.position - transform.right * 5f); // Extend line to the right
                }

                // The second position stays anchored to the object
                lineRenderer.SetPosition(1, transform.position);

                lineRenderer.SetPosition(0, transform.position + Vector3.right * (impactDirection.x > 0 ? 5f : -5f)); // Adjust for dash side
                lineRenderer.SetPosition(1, transform.position);


                for (int i = 0; i < wiggleCount; i++)
                {
                    // Wiggle to the right
                    Quaternion rightRotation = startRotation * Quaternion.Euler(0, 0, wiggleAngle);
                    float elapsed = 0f;
                    Quaternion leftRotation = startRotation * Quaternion.Euler(0, 0, -wiggleAngle);

                    while (elapsed < wiggleDuration)
                    {
                        elapsed += Time.deltaTime;
                        transform.rotation = Quaternion.Lerp(leftRotation, rightRotation, elapsed / wiggleDuration);

                       

                        yield return null;
                    }

                    // Wiggle to the left
                    elapsed = 0f;

                    while (elapsed < wiggleDuration)
                    {
                        elapsed += Time.deltaTime;
                        transform.rotation = Quaternion.Lerp(rightRotation, leftRotation, elapsed / wiggleDuration);



                        yield return null;
                    }
                }


                // Rotate the object smoothly over time
                float elapsedTime = 0f;
                while (elapsedTime < rotationTime)
                {
                    elapsedTime += Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationTime);
                    yield return null;
                }

                // Ensure the final rotation matches exactly
                transform.rotation = endRotation;

                // Optionally re-enable physics
                rb.isKinematic = false;

                // (Optional) Freeze position constraints if it should stay down
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                lineRenderer.enabled = false;
                var damage = 10;
                gameObject.GetComponent<HeatSystem>().ChangeHeat(damage);
                Debug.Log("Hit");
                yield return new WaitForSeconds(3f);
               
                Destroy(rb.gameObject);
            }
        }

    }
}
