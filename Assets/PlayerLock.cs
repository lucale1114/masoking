using Player;
using System.ComponentModel;
using UnityEngine;

public class LockXAxis : MonoBehaviour
{

        private Rigidbody2D rb;
    public float ySpeed = 5f; // Speed of movement along the y-axis
    public bool isLockingXAxis = false; // Whether the x-axis should be locked

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isLockingXAxis)
        {
            // Get the player's y-axis input
            float yInput = Input.GetAxis("Vertical");

            // Lock x position and move along y-axis only
            Vector2 newPosition = new Vector2(rb.position.x , rb.position.y + yInput * ySpeed  * Time.deltaTime);
            newPosition.x -= 10 * Time.deltaTime ;
            rb.MovePosition(newPosition);
        }
    }

    public void EnableLock()
    {
        GameObject varGameObject = GameObject.FindWithTag("Player");

        isLockingXAxis = true; // Enable lock
        varGameObject.GetComponent<Movement>().enabled = false;
    }

    public void DisableLock()
    {
        GameObject varGameObject = GameObject.FindWithTag("Player");
        isLockingXAxis = false; // Disable lock
        varGameObject.GetComponent<Movement>().enabled = true;

    }

}

