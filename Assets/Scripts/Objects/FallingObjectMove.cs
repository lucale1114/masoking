using UnityEngine;

namespace Objects
{
    public class FallingObjectMove : MonoBehaviour
    {
        public static int movespeed = 1;
        public Vector3 userDirection = Vector3.right;
        public FallingObjectCollision FallingObjectCollision;


        public void Start()
        {

            Destroy(gameObject,18f);
        }

        public void Update()
        {
            transform.Translate(userDirection * movespeed * Time.deltaTime);
            if (FallingObjectCollision.Dashed)
            {
                userDirection = Vector3.zero;
                movespeed = 0;
            }
        }



    }
}