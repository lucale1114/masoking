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

        public static bool BeenHit { get; set; } = false;
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

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                Player.Collision.BeenHit = true;
            }
        }
        private IEnumerator SwitchSceneDelay()
        {

            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            SceneManager.LoadScene("Level_1");

        }
    }
}