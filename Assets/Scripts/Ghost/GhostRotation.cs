using UnityEngine;

public class SharkRotation : MonoBehaviour
{
    public GhostWallBounce sharkMovement;

    private const float ROTATION_OFFSET = 0f;


    private void Update()
    {
        // Check if the movement component and a non-zero direction exist
        if (sharkMovement != null && sharkMovement.direction != Vector2.zero)
        {
            Vector2 direction = sharkMovement.direction;

            // 1. Handle Vertical Movement (Rotation)
            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                float angle = Mathf.Atan2(direction.y, direction.x);
                float rotationOffset = -90f;
                float finalAngle = angle * Mathf.Rad2Deg + rotationOffset;

                transform.rotation = Quaternion.AngleAxis(finalAngle, Vector3.forward);
            }

            else if (Mathf.Abs(direction.x) > 0.01f)
            {
                if (transform.rotation.z != 0)
                {
                    transform.rotation = Quaternion.identity;
                }

            }
        }
    }
}