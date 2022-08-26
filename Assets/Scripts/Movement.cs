using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] GhostEyes ghostEyes;
    [SerializeField] float speed = 8f;
    public float speedMultiplier = 1f;
    [SerializeField] Vector2 initialDirection;
    private LayerMask obstacleLayer;

    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private Vector3 right = new Vector3(0, 0, 0);
    private Vector3 up = new Vector3(0, 0, 90);
    private Vector3 left = new Vector3(0, 0, 180);
    private Vector3 down = new Vector3(0, 0, 270);

    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        obstacleLayer = LayerMask.GetMask(LayerMask.LayerToName(9));
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        if(gameManager.isGameStarted && !gameManager.gameOver)
        {
            gameManager.PlaySiren();
            Vector2 position = rigidbody.position;
            Vector2 translation = speed * speedMultiplier * Time.fixedDeltaTime * direction;

            rigidbody.MovePosition(position + translation);
        }
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
            if(gameObject.layer == LayerMask.NameToLayer("Pacman"))
            {
                RotatePacman();
            }
            if (gameObject.layer == LayerMask.NameToLayer("Ghost"))
            {
                ghostEyes.RotateEyes(direction);
            }
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

    public void RotatePacman()
    {
        if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(right); 
        } else if(direction == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(up);
        }
        else if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(left);
        }
        else if (direction == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(down);
        }
    }

}