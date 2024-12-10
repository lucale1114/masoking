using Misc;
using Player;
using UnityEngine;

namespace Objects
{
    public class DestroyObjectCollision : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private IntroUserInterface intro;
        [SerializeField] private AudioClip boom;

        bool hasDashed = false;

        public bool HasDashed => hasDashed;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && movement.IsCurrentlyDashing)

            {
                Collider2D collider2D = GetComponent<Collider2D>();
                Destroy(collider2D.gameObject);
                SoundFXManager.Instance.PlaySoundFX(boom, 1f);
            }
        }
    }
}