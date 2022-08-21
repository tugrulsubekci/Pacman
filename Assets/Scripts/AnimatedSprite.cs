using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public Sprite[] sprites;
    private float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    public bool loop = true;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(Advance),animationTime,animationTime);
    }

    private void Advance()
    {
        if(!spriteRenderer.enabled)
        {
            return;
        }
        animationFrame++;

        if(animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }

        if(animationFrame < sprites.Length && animationFrame >= 0)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }
}
