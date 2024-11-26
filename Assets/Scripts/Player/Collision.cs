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
       
        

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall") && !movement.isDashing)
            {
                movement.currentVelocity *= -2.5f;
            }
            if (other.gameObject.CompareTag("Destroy") && movement.IsCurentlyDashing )

            { 
             Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("DestroyIntro") && movement.IsCurentlyDashing && intro.HaveDash)

            {
                Destroy(other.gameObject);
                StartCoroutine(SwitchSceneDelay());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("DashableObject") && movement.IsCurentlyDashing)
            {
                StartCoroutine(FallOverCoroutine(collision.gameObject));
            }
        }

        private IEnumerator FallOverCoroutine(GameObject obj)
        {
            float rotationTime = 0.3f; // Duration of the fall
            float elapsed = 0f;
            Quaternion startRotation = obj.transform.rotation;
            Quaternion endRotation = Quaternion.Euler(0, 0, 90); // Rotate 90 degrees

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