using UnityEngine;

namespace Objects
{
    public class FallingObjectMove : MonoBehaviour
    {
        public static int movespeed = 1;
        public Vector3 userDirection = Vector3.right;
        public FallingObjectCollision FallingObjectCollision;
        public Animator animator;


        public void Start()
        {
            animator = GetComponent<Animator>();
            Destroy(gameObject,18f);
        }

        public void Update()
        {
            //animator.Play("");
            transform.Translate(userDirection * movespeed * Time.deltaTime);
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
            if (FallingObjectCollision.Dashed)
            {
                animator.SetBool("Idle",true);
                userDirection = Vector3.zero;
                movespeed = 0;
            }
        }



    }
}
