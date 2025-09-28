using UnityEngine;
using System.Collections;

public class TurtleMovement : MonoBehaviour
{
    // Movement speed (used for duration/time, not velocity)
    public float speed = 10f;

    // Size of one "step" or "tile" movement
    public float stepSize = 1f; // Adjust this based on your maze scale!

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection = Vector2.right;

    private bool isMoving = false;

    // Layer mask to check for walls (only checking the Default layer)
    private int wallMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Only check for objects on the "Default" layer (your walls)
        wallMask = LayerMask.GetMask("Default");

        // Start the continuous movement coroutine
        StartCoroutine(Move());
    }

    void Update()
    {
        // --- 1. Handle Input (Same as before) ---
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 inputDirection = Vector2.zero;

        if (x != 0)
        {
            inputDirection = new Vector2(x, 0);
        }
        else if (y != 0)
        {
            inputDirection = new Vector2(0, y);
        }

        if (inputDirection != Vector2.zero)
        {
            nextDirection = inputDirection;
        }
    }

    // Coroutine handles movement in discrete steps
    IEnumerator Move()
    {
        while (true) // Infinite loop to keep the turtle moving
        {
            // If the requested turn is valid, update direction
            if (CheckMove(nextDirection))
            {
                direction = nextDirection;
            }

            // Check if we can move in the current direction
            if (CheckMove(direction))
            {
                isMoving = true;

                // Calculate the target position
                Vector2 startPos = transform.position;
                Vector2 endPos = startPos + direction * stepSize;

                // Duration of the move based on speed
                float duration = stepSize / speed;
                float elapsed = 0f;

                // --- Movement Loop ---
                while (elapsed < duration)
                {
                    float t = elapsed / duration;
                    // Move using Lerp for smooth movement
                    rb.MovePosition(Vector2.Lerp(startPos, endPos, t));
                    elapsed += Time.deltaTime;
                    yield return null; 
                }

                // Ensure we snap exactly to the destination at the end
                rb.MovePosition(endPos);
                isMoving = false;

                // --- Rotation ---
                RotateTurtle(direction);
            }
            else
            {
                // If blocked, wait a tiny bit before checking input again
                yield return null;
            }
        }
    }

    // Checks if the path is clear for the given direction (similar to CanMove)
    bool CheckMove(Vector2 dir)
    {
        // Cast a small box slightly smaller than the turtle to check for walls.
        // This is a more forgiving check for collision
        RaycastHit2D hit = Physics2D.BoxCast(
            rb.position,
            GetComponent<CircleCollider2D>().bounds.size * 0.9f,
            0f,
            dir,
            stepSize * 0.5f, // Check halfway to the next tile
            wallMask);

        // If the BoxCast hits nothing, the path is clear.
        return hit.collider == null;
    }

    // Rotates the sprite to face the direction of movement
    void RotateTurtle(Vector2 currentDirection)
    {
        if (currentDirection == Vector2.right)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (currentDirection == Vector2.left)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (currentDirection == Vector2.up)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (currentDirection == Vector2.down)
            transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}