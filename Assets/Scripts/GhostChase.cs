using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }


    // GhostChase.cs - Updated OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;
            Vector2 reverseDirection = -ghost.movement.direction; // Get the direction we *cannot* move

            // Find the available direction that moves closet to pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // GHOSTS CANNOT TURN 180 DEGREES
                if (availableDirection == reverseDirection)
                {
                    continue; // Skip the reverse direction
                }

                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            // Safety check: if direction is still zero, it means only the reverse direction was available.
            if (direction == Vector2.zero)
            {
        
            }

            ghost.movement.SetDirection(direction);
        }
    }
    /*    private void OnTriggerEnter2D(Collider2D other)
        {
            Node node = other.GetComponent<Node>();

            // Do nothing while the ghost is frightened
            if (node != null && enabled)
            {
                Vector2 direction = Vector2.zero;
                float minDistance = float.MaxValue;

                // Find the available direction that moves closet to pacman
                foreach (Vector2 availableDirection in node.availableDirections)
                {
                    // If the distance in this direction is less than the current
                    // min distance then this direction becomes the new closest
                    Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                    float distance = (ghost.target.position - newPosition).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                }

                ghost.movement.SetDirection(direction);
            }
        }
    */
}