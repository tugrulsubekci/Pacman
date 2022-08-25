using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostScatter scatter { get; private set; }

    private GameManager gameManager;

    [SerializeField] GhostBehaviour initialBehaviour;
    public Transform target;

    public int points = 200;
    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
        scatter = GetComponent<GhostScatter>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        ResetState();
    }
    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        chase.Disable();
        frightened.Disable();

        scatter.Enable();

        if(home != initialBehaviour)
        {
            home.Disable();
        }

        if(initialBehaviour != null)
        {
            initialBehaviour.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if(frightened.enabled)
            {
                gameManager.GhostEaten(this);
            }
            else
            {
                gameManager.PacmanEaten();
            }
        }
    }
    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }
}
