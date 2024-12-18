using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public LineRenderer lineRenderer;  // Assign your Line Renderer in the Inspector
    public float moveSpeed = 2f;       // Speed of the object
    public GameObject child;

    private Vector3[] pathPoints;      // Array to hold path points
    private int currentPointIndex = 0; // Tracks the current target point


    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;



    void Start()
    {
        // Fetch all points from the Line Renderer
        int numPoints = lineRenderer.positionCount;
        pathPoints = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            // Convert local LineRenderer positions to world positions
            pathPoints[i] = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(i));
        }

        // Optionally, ensure the object starts at the first point
        if (pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
        }

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Record the initial position
        lastPosition = transform.position;
    }

    void Update()
    {
        if (pathPoints.Length == 0) return;

        // Move towards the current target point
        transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentPointIndex], moveSpeed * Time.deltaTime);

        // Check if we reached the current point, then move to the next
        if (Vector3.Distance(transform.position, pathPoints[currentPointIndex]) < 0.1f)
        {
            currentPointIndex++;
            // Check if we've reached the last point
            if (currentPointIndex >= pathPoints.Length)
            {
                // Destroy the object when the last point is reached
                Destroy(gameObject);
                OnDestroy();
            }
        }

        changeAnimation();

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

    void OnDestroy()
    {
        // Find all child objects and destroy them
        //child = GetComponent<ChainFollow>();
        Destroy(child.gameObject);
    }
}

