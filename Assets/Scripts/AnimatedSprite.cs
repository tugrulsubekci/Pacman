using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    [SerializeField] Sprite[] sprites;
    private float animationTime = 0.125f;
    public int animationFrame { get; private set; }
    [SerializeField] bool loop = true;
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
