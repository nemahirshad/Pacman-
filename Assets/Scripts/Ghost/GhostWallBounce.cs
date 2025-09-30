using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostWallBounce : MonoBehaviour
{
    [HideInInspector] public bool canWallBounce = true;

    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private Ghost ghost;

    // Inside GhostWallBounce.cs

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log($"Shark Rigidbody is: {rb}"); // Check 1: Should not be null

        ghost = GetComponent<Ghost>();
        Debug.Log($"Shark Ghost script is: {ghost}"); // Check 2: Should not be null

        startingPosition = transform.position;
        Debug.Log($"Shark Starting Position is: {startingPosition}"); // Check 3: Should not be null
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        // TEMPORARY FIX: Add this null check to find the crashing line (Line 47)
        if (rb == null)
        {
            Debug.LogError("Rigidbody is null on " + gameObject.name);
            return; // Prevent crash, but indicates a setup issue
        }

        // ... (Your other lines)
        transform.position = startingPosition;
        rb.isKinematic = false; // <-- The crashing line (or near it)
        enabled = true;
        canWallBounce = true;
        rb.velocity = Vector2.zero;
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

        rb.velocity = direction * currentSpeed;

    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit an obstacle/wall layer
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            rb.velocity = Vector2.zero;

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
    }*/


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