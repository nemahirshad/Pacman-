// Example: SharkRotation.cs or similar script on the shark GameObject
using UnityEngine;

public class SharkRotation : MonoBehaviour
{
    // Assign the movement component here (e.g., GhostWallBounce or Movement)
    // Make sure to set this in the Inspector or in Awake/Start
    public GhostWallBounce sharkMovement;

    // Adjust this if your shark sprite is drawn facing a different direction (e.g., right or up)
    private const float ROTATION_OFFSET = 0f;


    private void Update()
    {
        // Check if the movement component and a non-zero direction exist
        if (sharkMovement != null && sharkMovement.direction != Vector2.zero)
        {
            // 1. Calculate the angle in radians from the current direction vector.
            //    Atan2 returns the angle between the positive x-axis and the point (x, y).
            float angle = Mathf.Atan2(sharkMovement.direction.y, sharkMovement.direction.x);

            // 2. Convert radians to degrees, add the offset, and apply as rotation.
            //    The offset corrects the sprite's default orientation (often facing up, hence -90).
            float finalAngle = angle * Mathf.Rad2Deg + ROTATION_OFFSET;

            // 3. Apply the rotation around the Z-axis (Vector3.forward)
            transform.rotation = Quaternion.AngleAxis(finalAngle, Vector3.forward);
        }
    }
}