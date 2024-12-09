using Player;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Objects
{
    public class BombJester : MonoBehaviour
    {
        public static int movespeed = 2;
        
        public Vector3 userDirection = Vector3.right;
        public BombJesterCollision bombCollision;
        Rigidbody2D rb;
        float thrust = 40;
       
        //public Animator animator;


        public void Start()
        {
            //animator = GetComponent<Animator>();
            Destroy(gameObject, 18f);
            rb = GetComponent<Rigidbody2D>();
           
        }

        public void Update()
        {
            //animator.Play("");
            transform.Translate( Time.deltaTime * movespeed * userDirection);
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

         
            if  (bombCollision.HasDashed == true)
            {
               
                // animator.SetBool("Idle", true);
                transform.Translate(Time.deltaTime * movespeed * userDirection * thrust);
            }
        }

      


    }
}
