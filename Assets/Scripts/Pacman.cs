using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField] private AnimatedSprite deathSequence;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Movement movement;
    [SerializeField] private Sprite initialTurtleSprite;


    private void Awake()
    {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }

        //--------------------------------------------
        // Read the main joystick axis values (returns -1 to 1)
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // We check for absolute values close to 1 to detect a definite push.
        // Use a threshold (e.g., 0.9) to ignore slight stick drifts.
        const float threshold = 0.9f;

        // Horizontal Movement
        if (xInput > threshold)
        {
            // Right
            movement.SetDirection(Vector2.right);
        }
        else if (xInput < -threshold)
        {
            // Left
            movement.SetDirection(Vector2.left);
        }

        // Vertical Movement (Only check if not moving horizontally to prioritize clean turns)
        else if (yInput > threshold)
        {
            // Up
            movement.SetDirection(Vector2.up);
        }
        else if (yInput < -threshold)
        {
            // Down
            movement.SetDirection(Vector2.down);
        }

        //----------------------------------------------
        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        float rotationOffset = -90f;
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg + rotationOffset, Vector3.forward);
    }

    public void ResetState()
    {
        enabled = true;
        gameObject.SetActive(true);

        //spriteRenderer.enabled = true;
        if (spriteRenderer != null && initialTurtleSprite != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = initialTurtleSprite;
        }

        if (circleCollider != null)
        {
            circleCollider.enabled = true;
        }

        if (deathSequence != null)
        {
            deathSequence.enabled = false;

            deathSequence.gameObject.SetActive(false);
        }

        if (movement != null)
        {
            movement.ResetState();
        }


    }

    public void DeathSequence()
    {
        enabled = false;

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (circleCollider != null) circleCollider.enabled = false;
        if (movement != null) movement.enabled = false;

        if (deathSequence != null)
        {
            deathSequence.gameObject.SetActive(true);
        }
        if (deathSequence is AnimatedSprite anim)
        {
            anim.enabled = true;
            anim.Restart();
        }
    }

    private void OnEnable()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true; 
        }
    }

    private void OnValidate()
    {
        // This runs in the editor whenever a value is changed
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>(); // Try to fetch it if null
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
}