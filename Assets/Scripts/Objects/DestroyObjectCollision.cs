using Misc;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyObjectCollision : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private IntroUserInterface intro;
    [SerializeField] private AudioClip boom;
    


    bool hasDashed = false;

    public bool HasDashed => hasDashed;

    private void OnCollisionEnter2D(Collision2D other)
    {
       /* if (other.gameObject.CompareTag("Player") && movement.IsCurrentlyDashing)

        {
            Collider2D collider2D = GetComponent<Collider2D>();
            Destroy(collider2D.gameObject);
            SoundFXManager.Instance.PlaySoundFX(boom, transform, 1f);
        }*/

        if (other.gameObject.CompareTag("Player") && movement.IsCurrentlyDashing)

        {
            Debug.Log("Here");
            Collider2D collider2D = GetComponent<Collider2D>();
            SoundFXManager.Instance.PlaySoundFX(boom, transform, 1f);
            if (gameObject.CompareTag("DestroyIntro"))
            {
            StartCoroutine(SwitchSceneDelay());
            Destroy(gameObject);

            }

            else
            {
                Destroy(collider2D.gameObject);
            }

        }
    }
    IEnumerator SwitchSceneDelay()
    {
        Debug.Log("Haer");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level_1");
        Debug.Log("HEllo");

    }

}
