using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    [SerializeField] Sprite[] sprites;
    [SerializeField] float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    [SerializeField] bool loop = true;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled)
        {
            return;
        }
        animationFrame++;

        if (animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }

        if (animationFrame < sprites.Length && animationFrame >= 0)
        {
            spriteRenderer.sprite = sprites[animationFrame];
            if(spriteRenderer.sprite == sprites[sprites.Length - 1] && !loop)
            {
                CancelInvoke("Advance");
            }
        }
    }
    public void Restart()
    {
        animationFrame = -1;

        Advance();
    }
    private void OnDisable()
    {
        CancelInvoke("Advance");
        animationFrame = -1;
    }
}
