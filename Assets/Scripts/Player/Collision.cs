using System.Collections;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private IntroUserInterface intro;
        [SerializeField] private AudioClip boom;
        [SerializeField] private LineRenderer lineRenderer;

        
        bool hasDashed = false;
        
        public bool HasDashed => hasDashed;


        private void Awake()
        {
            
            lineRenderer.enabled = false;
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Destroy") && movement.IsCurrentlyDashing )

            {
                Destroy(other.gameObject);
                SoundFXManager.Instance.PlaySoundFX(boom, transform, 1f);
            }

            if (other.gameObject.CompareTag("DestroyIntro") && movement.IsCurrentlyDashing && intro.HaveDash)

            {
                Destroy(other.gameObject);
                SoundFXManager.Instance.PlaySoundFX(boom, transform, 1f);
                StartCoroutine(SwitchSceneDelay());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("DashableObject") && movement.IsCurrentlyDashing)
            {
                StartCoroutine(FallOver(collision.gameObject, movement.transform.position));

            }
        }

        private IEnumerator FallOver(GameObject obj, Vector2 playerPosition)
        {

            hasDashed = true;


            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (hasDashed != false)
                {


                    // Temporarily disable Rigidbody2D to manually animate the fall
                    rb.isKinematic = true;

                    // Calculate the fall direction based on player position
                    Vector2 impactDirection = (obj.transform.position - (Vector3)playerPosition).normalized;
                    float fallAngle = impactDirection.x > 0 ? -90f : 90f; // Right or left fall
                    float rotationTime = 0.8f; // Time it takes to fall
                    Quaternion startRotation = obj.transform.rotation;
                    Quaternion endRotation = Quaternion.Euler(0, 0, fallAngle);

                    float wiggleAngle = 7;
                    float wiggleCount = 6f;
                    float wiggleDuration = 0.25f;

                    lineRenderer.enabled = true;
                    lineRenderer.transform.rotation = Quaternion.identity;
               


                    for (int i = 0; i < wiggleCount; i++)
                    {
                        // Wiggle to the right
                        Quaternion rightRotation = startRotation * Quaternion.Euler(0, 0, wiggleAngle);
                        float elapsed = 0f;
                        Quaternion leftRotation = startRotation * Quaternion.Euler(0, 0, -wiggleAngle);

                        while (elapsed < wiggleDuration)
                        {
                            elapsed += Time.deltaTime;
                            obj.transform.rotation = Quaternion.Lerp(leftRotation, rightRotation, elapsed / wiggleDuration);
                            yield return null;
                        }

                        // Wiggle to the left
                        elapsed = 0f;

                        while (elapsed < wiggleDuration)
                        {
                            elapsed += Time.deltaTime;
                            obj.transform.rotation = Quaternion.Lerp(rightRotation, leftRotation, elapsed / wiggleDuration);
                            yield return null;
                        }
                    }


                    // Rotate the object smoothly over time
                    float elapsedTime = 0f;
                    while (elapsedTime < rotationTime)
                    {
                        elapsedTime += Time.deltaTime;
                        obj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationTime);
                        yield return null;
                    }

                    // Ensure the final rotation matches exactly
                    obj.transform.rotation = endRotation;

                    // Optionally re-enable physics
                    rb.isKinematic = false;

                    // (Optional) Freeze position constraints if it should stay down
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;

                    lineRenderer.enabled = false;

                    yield return new WaitForSeconds(3f);
                    Destroy(obj);
                }
            }

        }

        private IEnumerator SwitchSceneDelay()
        {

            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            SceneManager.LoadScene("Level_1");

        }
    }
}