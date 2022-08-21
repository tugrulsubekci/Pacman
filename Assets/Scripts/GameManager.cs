using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }
    private void Start()
    {
        NewGame();
    }
    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }
    private void NewRound()
    {
        foreach(Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(true);
        }

        pacman.gameObject.SetActive(true);
    }
    private void GameOver()
    {

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }
    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        SetScore(score + ghost.points);
    }
    public void PacmanEaten()
    {
        pacman.gameObject.SetActive(false);

        SetLives(lives - 1);

        if(lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            NewRound();
        }
    }
}
