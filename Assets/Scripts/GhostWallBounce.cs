using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostWallBounce : MonoBehaviour
{
    // NEW FLAG: This is controlled by GhostBehavior scripts (Chase, Scatter, Frightened).
    // When false, the OnCollisionEnter2D logic is bypassed, giving control to the Node logic.
    [HideInInspector] public bool canWallBounce = true;

    // Properties shared with the original Movement script
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private Ghost ghost;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ghost = GetComponent<Ghost>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rb.isKinematic = false;
        enabled = true;
        canWallBounce = true; // Reset to default bounce behavior
        rb.velocity = Vector2.zero; // IMPORTANT: Clear velocity on reset
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        float currentSpeed = speed * speedMultiplier;

        // Use Rigidbody velocity for controlled movement, essential for Kinematic body interaction
        // with the physics engine (this prevents "teleporting" via rb.MovePosition if physics takes over)
        rb.velocity = direction * currentSpeed;

        // Note: The previous rb.MovePosition is less ideal for a Kinematic body used with collisions.
        // We rely on rb.velocity now.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit an obstacle/wall layer
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            // NEW LOGIC: Stop ALL movement immediately on hitting a wall
            rb.velocity = Vector2.zero;

            // ----------------------------------------------------------------
            // LOGIC FOR NON-CHASE BEHAVIORS (Scatter/Frightened)
            // ----------------------------------------------------------------
            if (canWallBounce)
            {
                Node currentNode = GetCurrentNode();

                if (currentNode != null)
                {
                    // At a node, find a new random path (Scatter/Frightened logic)
                    Vector2 newDirection = GetNewRandomDirection(currentNode);
                    SetDirection(newDirection, forced: true);
                }
                else
                {
                    // Not at a node, just reverse direction (simple wall bounce)
                    SetDirection(-direction, forced: true);
                }

                // IMPORTANT: The snap back is no longer needed since we used rb.velocity = 0
                // and the collision naturally prevents penetration.
            }
            else
            {
                // ----------------------------------------------------------------
                // LOGIC FOR CHASE BEHAVIOR
                // ----------------------------------------------------------------
                // When Chase is active (canWallBounce is false), the Ghost must rely 
                // entirely on the Node trigger (OnTriggerEnter2D in GhostChase.cs) 
                // to find a new direction. Hitting a wall outside a node means 
                // the Ghost should simply stop and wait for the Node trigger to update direction.
                SetDirection(Vector2.zero, forced: true); // Force direction to zero until node logic runs
            }
        }
    }

    // ... (GetCurrentNode, GetNewRandomDirection, SetDirection, Occupied remain unchanged)

    // Helper function to check if the ghost is currently inside a Node trigger
    private Node GetCurrentNode()
    {
        // This relies on the Node having an IsTrigger collider
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D hit in hits)
        {
            Node node = hit.GetComponent<Node>();
            if (node != null)
            {
                return node;
            }
        }
        return null;
    }

    // Helper function to get a random valid direction (not 180 degrees opposite)
    private Vector2 GetNewRandomDirection(Node node)
    {
        // This logic is primarily for Scatter or Frightened mode.
        List<Vector2> validDirections = node.availableDirections
            .Where(d => d != -direction)
            .ToList();

        if (validDirections.Count > 0)
        {
            int randomIndex = Random.Range(0, validDirections.Count);
            return validDirections[randomIndex];
        }

        // If the only path available is backward, take it.
        return -direction;
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        this.direction = direction;
        nextDirection = Vector2.zero;

        // Sprite flip for visual direction
        if (direction == Vector2.left) { transform.localScale = new Vector3(-1f, 1f, 1f); }
        else if (direction == Vector2.right) { transform.localScale = new Vector3(1f, 1f, 1f); }
    }

    public bool Occupied(Vector2 direction)
    {
        // This script relies on physics collision (OnCollisionEnter2D) and Node triggers.
        return false;
    }
}