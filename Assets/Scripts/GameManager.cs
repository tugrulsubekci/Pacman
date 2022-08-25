using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    // UI
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] List<GameObject> extraLives;
    [SerializeField] GameObject gameOverPanel;

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        LoadHighScore();
        SetScore(0);
        SetLives(3);
        NewRound();
        gameOverPanel.SetActive(false);
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
        ResetGhostMultiplier();
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = this.score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        UpdateLives();
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
        if(ghostMultiplier == 5 && lives < 5)
        {
            SetLives(this.lives + 1);
        }
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
            gameOverPanel.SetActive(true);
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);
        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            SaveHighScore(score);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet powerPellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(powerPellet.duration);
        }

        PelletEaten(powerPellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), powerPellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if(pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private void UpdateLives()
    {
        foreach (GameObject live in extraLives)
        {
            live.SetActive(false);
        }
        for (int i = 0; i < lives - 1; i++)
        {
            extraLives[i].SetActive(true);
        }
    }

    private void SaveHighScore(int score)
    {
        if(score > DataManager.Instance.highScore)
        {
            DataManager.Instance.highScore = score;
            DataManager.Instance.Save();
            RefreshHighScore();
        }
    }

    private void LoadHighScore()
    {
        DataManager.Instance.Load();
        score = DataManager.Instance.highScore;
        RefreshHighScore();
    }

    private void RefreshHighScore()
    {
        highScoreText.text = score.ToString();
    }
}
