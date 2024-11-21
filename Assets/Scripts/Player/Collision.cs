using UnityEngine;

namespace Player
{
    public class Collision : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        private Player.Movement movementTarget;
        

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                movement.currentVelocity = Vector2.zero;
            }
            if (other.gameObject.CompareTag("Destroy") && movement.IsCurentlyDashing )

            { 
             Destroy(other.gameObject);
            }
        }
 
    }
}