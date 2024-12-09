using Player;
using UnityEngine;

namespace Jester
{
    public class BombJesterCollision : MonoBehaviour
    {
        Rigidbody2D rb;
        public Movement movement;
        public Transform myChildObject;
        public bool detachChild;
        private bool hasDashed = false;

        public bool HasDashed => hasDashed;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            detachChild = false;
        }

        // Update is called once per frame
        void Update()
        {

            if (detachChild == true)
            {
                if (myChildObject != null)
                {
                    transform.parent = null;
                    myChildObject.parent = null;
                }
            }


        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && movement.IsCurrentlyDashing)
            {
                hasDashed = true;
                detachChild = true;

            }


        }

    }
}