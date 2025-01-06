using System.Collections;
using UnityEngine;
using DG.Tweening;
using Player;

namespace Jester
{
    public class BombJesterNew : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");

        [Header("Jester Settings")]
        [SerializeField] private float thrust = 15f; // Force at which the Jester is launched
        [SerializeField] private AudioClip[] smackSounds; // Jester smack sounds

        [Header("Bomb Settings")]
        [SerializeField] private Transform bombTransform; // Assign the bomb in the inspector
        [SerializeField] private GameObject wallDashAnimationPrefab; // Optional dash effect prefab

        private GameObject _player;
        private Animator _animator;
        private Rigidbody2D _rb;

        private bool _launched; // Tracks if the Jester has already been launched

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the Player dashed into the Jester
            if (collision.CompareTag("Player") && !_launched)
            {
                var movement = collision.GetComponent<Movement>();
                if (movement != null && movement.IsCurrentlyDashing)
                {
                    HandleDashCollision(collision);
                }
            }
        }

        private void HandleDashCollision(Collider2D collision)
        {
            _launched = true; // Mark the Jester as launched

            // Play a random smack sound
            if (smackSounds.Length > 0)
            {
                SoundFXManager.Instance.PlayRandomSoundFX(smackSounds, 1f);
            }

            // Recoil the player slightly backward
            ApplyPlayerRecoil(collision);

            // Detach and activate the bomb
            DetachAndActivateBomb();

            // Launch the Jester away from the player
            LaunchJester();

            // Optional: Instantiate a wall dash animation (if assigned)
            if (wallDashAnimationPrefab != null)
            {
                Instantiate(wallDashAnimationPrefab, transform.position, Quaternion.identity);
            }
        }

        private void ApplyPlayerRecoil(Collider2D collision)
        {
            // Get the player's Rigidbody2D
            var playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Calculate recoil direction (opposite of Jester's position)
                Vector2 recoilDirection = (collision.transform.position - transform.position).normalized;

                // Apply a small recoil force
                float recoilForce = 3f; // Adjust this value as needed
                playerRb.velocity = recoilDirection * recoilForce;
            }
        }

        private void DetachAndActivateBomb()
        {
            if (bombTransform != null)
            {
                // Detach the bomb
                bombTransform.parent = null;

                // Activate the bomb
                var bombScript = bombTransform.GetComponent<Bomb>();
                if (bombScript != null)
                {
                    bombScript.Activate();
                }
            }
        }

        private void LaunchJester()
        {
            // Calculate launch direction (away from the player)
            Vector2 launchDirection = (transform.position - _player.transform.position).normalized;

            // Apply thrust to the Jester's Rigidbody2D
            _rb.velocity = launchDirection * thrust;

            // Destroy the Jester after 2 seconds
            Destroy(gameObject, 2f);
        }
    }
}

