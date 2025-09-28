using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GhostScatter : GhostBehavior
{
    private void OnDisable()
    {
        ghost.chase.Enable();
    }

    // GhostScatter.cs - Updated OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            // 1. Filter out the reverse direction
            List<Vector2> availableDirections = node.availableDirections
                .Where(d => d != -ghost.movement.direction)
                .ToList();

            // 2. If we only have the reverse direction available, we must take it (shouldn't happen often)
            if (availableDirections.Count == 0)
            {
                availableDirections.Add(-ghost.movement.direction);
            }

            // 3. Pick a random direction from the valid ones
            int index = Random.Range(0, availableDirections.Count);
            Vector2 direction = availableDirections[index];

            ghost.movement.SetDirection(direction);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && enabled)
        {
            // Pick a random available direction
            int index = Random.Range(0, node.availableDirections.Count);

            // Prefer not to go back the same direction so increment the index to
            // the next available direction
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++;

                // Wrap the index back around if overflowed
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
*/
}