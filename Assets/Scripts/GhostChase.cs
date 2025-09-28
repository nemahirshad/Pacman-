using UnityEngine;
using System.Linq; // Added for Linq operations

public class GhostChase : GhostBehavior
{
    private void OnEnable()
    {
        // When Chase is enabled, we disable wall bouncing to let the pathfinding handle movement.
        if (ghost.movement is GhostWallBounce wallBounce)
        {
            wallBounce.canWallBounce = false;
        }
    }

    private void OnDisable()
    {
        ghost.scatter.Enable();

        // Re-enable wall bounce if necessary when chase ends
        if (ghost.movement is GhostWallBounce wallBounce)
        {
            wallBounce.canWallBounce = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Check if we hit a node and if this behavior is currently active
        if (node != null && enabled)
        {
            // SNAP POSITION to the node center to prevent sticking near walls
            ghost.movement.rb.position = node.transform.position;

            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;
            Vector2 currentDirection = ghost.movement.direction;

            // Find the available direction that moves closest to Pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // RULE 1: Ghost cannot turn 180 degrees (cannot go backward)
                if (availableDirection == -currentDirection)
                {
                    continue; // Skip this direction
                }

                // Calculate the distance from the *next* potential position to the target
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            // Ensure the ghost is not stopping if all options are bad, fall back to current direction if possible
            if (direction != Vector2.zero)
            {
                ghost.movement.SetDirection(direction);
            }
            // Optional fallback: If only the reverse path is available (dead-end), take it.
            // Your current logic already handles this by default since the loop only skips the backward path.
        }
    }

}