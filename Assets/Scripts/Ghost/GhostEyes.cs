
using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;
    public void RotateEyes(Vector2 direction)
    {
        if (direction == Vector2.right)
        {
            spriteRenderer.sprite = right;
        }
        else if (direction == Vector2.up)
        {
            spriteRenderer.sprite = up;
        }
        else if (direction == Vector2.left)
        {
            spriteRenderer.sprite = left;
        }
        else if (direction == Vector2.down)
        {
            spriteRenderer.sprite = down;
        }
    }
}
