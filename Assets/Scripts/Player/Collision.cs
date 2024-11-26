using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Misc;


namespace Player
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private IntroUserInterface intro;
        [SerializeField] private AudioClip boom;



        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall") && !movement.isDashing)
            {
                movement.currentVelocity *= -2.5f;
            }
            if (other.gameObject.CompareTag("Destroy") && movement.IsCurentlyDashing )

            { 
                Destroy(other.gameObject);
                SoundFXManager.Instance.PlaySoundFX(boom, transform, 1f);
            }

            if (other.gameObject.CompareTag("DestroyIntro") && movement.IsCurentlyDashing && intro.HaveDash)

            {
                Destroy(other.gameObject);
                SoundFXManager.Instance.PlaySoundFX(boom, transform, 1f);
                StartCoroutine(SwitchSceneDelay());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("DashableObject") && movement.IsCurentlyDashing)
            {
                StartCoroutine(FallOverCoroutine(collision.gameObject, movement.transform.position));
            }
        }

        private IEnumerator FallOverCoroutine(GameObject obj, Vector3 playerPosition)
        {
            float rotationTime = 0.5f; // Duration of the fall
            float elapsed = 0f;
            Quaternion startRotation = obj.transform.rotation;

            // Determine the fall direction
            Vector3 objectPosition = obj.transform.position;
            bool fallToRight = movement.transform.position.x < objectPosition.x; // Change to y for vertical fall

            // Set the target rotation based on the direction
            Quaternion endRotation = fallToRight
                ? Quaternion.Euler(0, 0, -90) // Fall to the right (clockwise)
                : Quaternion.Euler(0, 0, 90); // Fall to the left (counterclockwise)

            while (elapsed < rotationTime)
            {
                elapsed += Time.deltaTime;
                obj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / rotationTime);
                yield return null;
            }
        }

        private IEnumerator SwitchSceneDelay()
        {

            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            SceneManager.LoadScene("Level_1");

        }
    }
}