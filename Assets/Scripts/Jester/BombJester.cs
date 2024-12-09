
using Player;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.WSA;

namespace Objects
{
    public class BombJester : MonoBehaviour
    {
        public static int movespeed = 2;
        
        public Vector3 userDirection = Vector3.right;
        public BombJesterCollision bombCollision;
        Rigidbody2D rb;
        [SerializeField] float thrust = 15;
        public static GameObject player;
        private bool launched = false;
        GameObject childObject;
        [SerializeField] Vector3 bombOffset = new Vector3(0.175f, 0.67f, 0); // Offset for bomb position


        //public Animator animator;


        public void Start()
        {
            //animator = GetComponent<Animator>();
            Destroy(gameObject, 18f);
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            childObject = GameObject.FindGameObjectWithTag("Bomb");

            if (childObject != null)
            {
                // Set initial bomb position relative to jester
                childObject.transform.position = transform.position + bombOffset;

                    childObject.transform.parent = null; // Detach from jester

                

            }


        }

        public void Update()
        {
            //animator.Play("");
            if (!launched)
            {
                // Regular Jester movement
                rb.velocity = userDirection * movespeed;

                // Bomb follows Jester until launch
                if (childObject != null)
                {
                    childObject.transform.position = transform.position + bombOffset;
                }
            }

            if (bombCollision.HasDashed && !launched)
            {
                if (childObject != null)
                {

                }
                launched = true;
                Launch();
            }
            if (userDirection.x > 0)
            {
                // Moving left: flip sprite
                transform.localScale = new Vector3(-0.35f, 0.35f, 0); // Flip along the X-axis
            }
            else if (userDirection.x < 0)
            {
                // Moving right: reset sprite
                transform.localScale = new Vector3(0.35f, 0.35f, 0);
            }
        }

        private void Launch()
        {
            //userDirection = Vector3.zero;
            //movespeed = 0;
            childObject.transform.position = Vector3.zero;
            Vector3 launchDirection =(transform.position - player.transform.position).normalized;
            rb.velocity = thrust * launchDirection;
            if (childObject != null)
            {
                // Bomb remains in its last position
                childObject.transform.position = transform.position + bombOffset;
                childObject = null; // Stop tracking the bomb
            }

            Destroy(rb, 4f);
            

        }


    }
}
