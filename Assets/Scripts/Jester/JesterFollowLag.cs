using UnityEngine;
using System.Collections.Generic;

public class ChainFollow : MonoBehaviour
{
    public Transform targetToFollow;     // The object this one will follow (parent or previous child)
    public float followSpeed = 5f;       // Speed of the follow movement
    public float followDistance = 0.1f;    // Desired distance between objects

    private Queue<Vector3> positionHistory = new Queue<Vector3>();

    void Update()
    {
        if (targetToFollow == null) return;

        // Record the target's position over time
        positionHistory.Enqueue(targetToFollow.position);

        // Ensure we keep a consistent distance by storing enough positions
        if (positionHistory.Count > Mathf.RoundToInt(followDistance / (followSpeed * Time.deltaTime)))
        {
            Vector3 nextPosition = positionHistory.Dequeue(); // Get the next position to move to
            transform.position = Vector3.Lerp(transform.position, nextPosition, followSpeed * Time.deltaTime);
        }
        changeAnimation();
    }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Record the initial position
        lastPosition = transform.position;

        Destroy(gameObject, 31f);
      
    }

    void changeAnimation()
    {
        // Calculate direction based on movement
        float deltaX = transform.position.x - lastPosition.x;

        // Check if moving left or right
        if (deltaX > 0.01f) // Moving right
        {
            spriteRenderer.flipX = true; // Ensure sprite faces right
        }
        else if (deltaX < -0.01f) // Moving left
        {
            spriteRenderer.flipX = false; // Flip sprite to face left
        }

        // Update last position
        lastPosition = transform.position;
    }
}


