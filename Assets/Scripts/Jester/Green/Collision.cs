using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Jester.Green
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private AudioClip[] fall;
        [SerializeField] private AudioClip[] smash;
        [SerializeField] private GameObject _wallDashAnimationPrefab;

        private Collider2D triggerCollider;
        private Player.Movement movement;
        public Animator animator;
        private GameObject _player;

        public bool Dashed => dashed;

        bool hasDashed = false;
        bool dashed = false;
        bool isFalling = false;
        bool beenHit = false;
        bool hasFallen = false;

        public bool HasDashed => hasDashed;


        private void Awake()
        {
            lineRenderer.enabled = false;
            triggerCollider = GetComponent<Collider2D>();
           

        }

        private void Start()
        {
            movement = FindObjectOfType<Player.Movement>();
            animator = GetComponent<Animator>();
            _player = GameObject.Find("Player");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && movement.IsCurrentlyDashing)
            {
                
                StartCoroutine(FallOver(this.gameObject, collision.transform.position));
                // Get the Movement component
                movement = collision.GetComponent<Player.Movement>();

                // Get the player's Rigidbody2D
                Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Calculate the bounce direction (normal)
                    Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;

                    // Apply the bounce velocity
                    float bounceForce = 10f; // Adjust this value for desired bounce strength
                    playerRb.velocity = bounceDirection * bounceForce;
                }

                // Instantiate wall dash animation at the collision point
                Instantiate(_wallDashAnimationPrefab,
                    transform.GetComponent<Collider2D>().ClosestPoint(collision.transform.position),
                    Quaternion.identity);
                animator.SetTrigger("Hit");
                SoundFXManager.Instance.PlayRandomSoundFX(fall, 1f);
            }

            if (collision.gameObject.CompareTag("Player") && isFalling != true && dashed == true && beenHit != true &&
                hasFallen != true)
            {
                var damage = 40;
                HeatSystem playerHeat = collision.gameObject.GetComponent<HeatSystem>();

                if (playerHeat != null)
                {
                    playerHeat.ChangeHeat(damage);
                    beenHit = true;
                }
                else
                {
                    UnityEngine.Debug.LogWarning("HeatSystem not found on Player!");
                }
            }
        }

        private IEnumerator FallOver(GameObject gameObject, Vector2 playerPosition)
        {
            hasDashed = true;


            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                if (hasDashed != false && dashed != true)
                {
                    _player.GetComponent<Player.Movement>().enabled = false;
                    animator.SetBool("Wobble", true);
                    dashed = true;
                    // Temporarily disable Rigidbody2D to manually animate the fall
                    rb.isKinematic = true;

                    // Calculate the fall direction based on player position
                    Vector2 impactDirection = (gameObject.transform.position - (Vector3)playerPosition).normalized;
                    float fallAngle = impactDirection.x > 0 ? -90f : 90f; // Right or left fall
                    float rotationTime = 0.5f; // Time it takes to fall
                    Quaternion startRotation = gameObject.transform.rotation;
                    Quaternion endRotation = Quaternion.Euler(0, 0, fallAngle);

                    float wiggleAngle = 7;
                    float wiggleCount = 1.5f;
                    float wiggleDuration = 0.25f;

                    lineRenderer.enabled = true;
                    lineRenderer.useWorldSpace = true;

                    triggerCollider.enabled = false;
                    isFalling = true;

                    if (impactDirection.x > 0) // Dash from left
                    {
                        lineRenderer.SetPosition(0, transform.position + transform.right * 3f);
                        lineRenderer.SetPosition(1, transform.position);
                    }
                    else // Dash from right
                    {
                        lineRenderer.SetPosition(0, transform.position - transform.right * 3f);
                    }

                    // The second position stays anchored to the object
                    lineRenderer.SetPosition(1, transform.position);

                    lineRenderer.SetPosition(0,
                        transform.position +
                        Vector3.right * (impactDirection.x > 0 ? 2.4f : -2.4f)); // Adjust for dash side
                    lineRenderer.SetPosition(1, transform.position);

                    yield return new WaitForSeconds(0.2f);
                    _player.GetComponent<Player.Movement>().enabled = true;

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
                    triggerCollider.enabled = true;
                    isFalling = false;
                    animator.SetBool("Wobble", false);
                    animator.SetTrigger("Hit");
                    SoundFXManager.Instance.PlayRandomSoundFX(smash, 2f);
                    yield return new WaitForSeconds(0.1f);

                    hasFallen = true;

                    yield return new WaitForSeconds(2f);

                    Destroy(rb.gameObject);
                }
            }
        }
    }
}