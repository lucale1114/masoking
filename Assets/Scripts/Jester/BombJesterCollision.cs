using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Jester
{
    public class BombJesterCollision : MonoBehaviour
    {
        private Movement _movement;
        private Transform _bomb;
        [SerializeField] private GameObject _wallDashAnimationPrefab;
        private GameObject _player;



        public bool HasDashed { get; private set; }

        private void Start()
        {
            _player = GameObject.Find("Player");

            _movement = GameObject.Find("Player").GetComponent<Movement>();
            _bomb = gameObject.transform.GetChild(0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && _movement.IsCurrentlyDashing)
            {
                GetComponentInChildren<Bomb>().Activate();
                // Get the Movement component
                _movement = collision.GetComponent<Player.Movement>();

                _player.GetComponent<Player.Movement>().enabled = false;


                // Get the player's Rigidbody2D
                Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Calculate the bounce direction (normal)
                    Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;

                    // Apply the bounce velocity
                    float bounceForce = 1.5f; // Adjust this value for desired bounce strength
                    playerRb.velocity = bounceDirection * bounceForce;
                }

                // Instantiate wall dash animation at the collision point
                Instantiate(_wallDashAnimationPrefab,
                    transform.GetComponent<Collider2D>().ClosestPoint(collision.transform.position),
                    Quaternion.identity);

                StartCoroutine(WaitSec());
                HasDashed = true;
                _bomb.parent = null;
            }
        }
    private IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(0.2F);
        _player.GetComponent<Player.Movement>().enabled = true;


    }
    }


}
