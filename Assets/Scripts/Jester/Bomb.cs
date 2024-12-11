using Player;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Jester
{
    public class Bomb : MonoBehaviour
    {
        public BombJesterCollision BombJesterCollision;


        public static GameObject _player;
        private Rigidbody2D _body;
        private float thrust = 3;
        [SerializeField] AudioClip[] booms;
        [SerializeField] Animator animator;

        [SerializeField] private float explosionRadius = 5f;

        //Vector3 playerPos = Player.transform.position;
        //Vector3 bombPos = bomb.transform.position;
        void Start()
        {
            // Find the player using a tag if not assigned
            _player = GameObject.FindGameObjectWithTag("Player");
            _player.GetComponent<Rigidbody2D>();


            if (_player == null)
            {
                UnityEngine.Debug.LogError("Player not assigned or found!");
            }
        }


        public void Update()
        {
            if (BombJesterCollision.HasDashed)
            {
                animator.SetBool("count", true);
                StartCoroutine(AnimationExplosion());
                Invoke(nameof(WaitForExplosion), 3f);
            }
        }

        public IEnumerator AnimationExplosion()
        {
            yield return new WaitForSeconds(2.8f);
            animator.SetBool("boom", true);

        }

        public void WaitForExplosion()
        {
            SoundFXManager.Instance.PlayRandomSoundFX(booms, 1f);

            if (_player != null)
            {
                // Calculate distance between bomb and player
                //float distance = Vector3.Distance(transform.position, player.transform.position);
                float sqrDistance = Vector3.SqrMagnitude(transform.position - _player.transform.position);


                if (sqrDistance <= explosionRadius * explosionRadius)
                {
                    UnityEngine.Debug.Log("Player is in range of the explosion!");
                    // Apply damage
                    HeatSystem playerHeat = _player.GetComponent<HeatSystem>();
                    var launchDirection = (transform.position - _player.transform.position).normalized;
                    //_player. = thrust * launchDirection;
                    if (playerHeat != null)
                    {
                        int damage = 10;
                        playerHeat.ChangeHeat(damage);
                        UnityEngine.Debug.Log($"Player took {damage} damage from the explosion.");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("HeatSystem component not found on Player!");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Player is out of range of the explosion.");
                }
            }
            Destroy(gameObject);
        }
    }
}