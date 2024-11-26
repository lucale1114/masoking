using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


namespace Player
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        private Player.Movement movementTarget;
        

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

            if (other.gameObject.CompareTag("DestroyIntro") && movement.IsCurentlyDashing)

            {
                Destroy(other.gameObject);
                StartCoroutine(SwitchSceneDelay());
            }
        }

        private IEnumerator SwitchSceneDelay()
        {

            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            SceneManager.LoadScene("Level_1");

        }
    }
}