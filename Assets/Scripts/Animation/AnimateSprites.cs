using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class AnimatedSprite : MonoBehaviour

{

    public Sprite[] sprites = new Sprite[0];

    public float animationTime = 0.25f;

    public bool loop = true;


    private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;



    private void Awake()

    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            defaultSprite = sprites[0];
        }
    }


    private void Start()
    {
        /*
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; 
        }
        */
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }


    private void Advance()

    {

        if (!spriteRenderer.enabled)

        {

            return;

        }



        animationFrame++;



        if (animationFrame >= sprites.Length && loop)

        {

            animationFrame = 0;

        }



        if (animationFrame >= 0 && animationFrame < sprites.Length)

        {

            spriteRenderer.sprite = sprites[animationFrame];

        }

    }



    public void Restart()

    {
        animationFrame = -1;
        Advance();

    }

    public void ResetSprite()
    {
        CancelInvoke(nameof(Advance));

        enabled = false;

        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.enabled = false; 
        }
    }

    public void HideSprite()
    {
        spriteRenderer.enabled = false;
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
            transform.localScale = Vector3.one; 
        }
    }

}