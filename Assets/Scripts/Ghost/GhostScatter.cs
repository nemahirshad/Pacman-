using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GhostScatter : GhostBehavior
{

    private void OnEnable()
    {
        // 1. Find the current Node the Ghost is sitting on (or near)
        // NOTE: You must ensure your Ghost object is tagged/layered correctly for this check to work.
        Node currentNode = FindCurrentNode();

        if (currentNode != null)
        {
            // 2. Filter available directions (excluding the reverse direction)
            List<Vector2> availableDirections = currentNode.availableDirections
                 .Where(d => d != -ghost.movement.direction)
                 .ToList();

            // 3. Fallback: If only reverse is available, take it.
            if (availableDirections.Count == 0)
            {
                availableDirections.Add(-ghost.movement.direction);
            }

            // 4. Pick a random direction from the valid ones and set movement
            int index = Random.Range(0, availableDirections.Count);
            Vector2 direction = availableDirections[index];

            ghost.movement.SetDirection(direction);
        }
        else
        {
            // Fallback if the Ghost is NOT on a node (e.g., inside the initial pen)
            // Use the direction set in the Inspector (which you must ensure is non-zero).
            ghost.movement.SetDirection(ghost.movement.initialDirection);
        }
    }

    private Node FindCurrentNode()
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