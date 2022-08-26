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

    public bool isGameStarted;
    public bool gameOver;
    public bool isFrightenedActive;
    private AudioManager audioManager;
    [SerializeField] AnimatedSprite aliveAnim;
    [SerializeField] AnimatedSprite deathAnim;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
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
        Invoke(nameof(StartGame),4.3f);
    }

    private void NewRound()
    {
        audioManager.Play("Beginning");
        foreach(Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        Invoke("GameOverFalse",3.0f);
        deathAnim.enabled = false;
        aliveAnim.enabled = true;
        ResetGhostMultiplier();
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOverFalse()
    {
        gameOver = false;
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
        audioManager.Play("EatGhost");
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
        if(ghostMultiplier == 5 && lives < 5)
        {
            audioManager.Play("ExtraPac");
            SetLives(this.lives + 1);
        }
    }

    public void PacmanEaten()
    {
        audioManager.Play("Death");
        audioManager.Stop("Siren");
        deathAnim.enabled = true;
        aliveAnim.enabled = false;
        gameOver = true;
        SetLives(lives - 1);

        if(lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            Invoke(nameof(ActivateGameOverPanel), 3.0f);
        }
    }
    private void ActivateGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void PelletEaten(Pellet pellet)
    {
        if (!audioManager.sounds[4].source.isPlaying)
        {
            audioManager.Play("Munch");
        }
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
        audioManager.Play("PowerPellet");
        audioManager.Stop("Siren");
        isFrightenedActive = true;
        CancelInvoke(nameof(StopPowerPelletMusic));
        Invoke(nameof(StopPowerPelletMusic), 8.0f);
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

    private void StartGame()
    {
        isGameStarted = true;
    }

    private void StopPowerPelletMusic()
    {
        isFrightenedActive = false;
        audioManager.Stop("PowerPellet");
    }

    public void PlaySiren()
    {
        if (!audioManager.sounds[5].source.isPlaying && !isFrightenedActive)
        {
            audioManager.Play("Siren");
        }
    }

}
