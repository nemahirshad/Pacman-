using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private Transform hazards;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public int score { get; private set; } = 0;


    private int ghostMultiplier = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        yield return null;

        NewGame();
    }
    private void Update()
    {

        if (gameOverText.enabled && Input.anyKeyDown)
        {
            NewGame();
        }

    }

    public void NewGame()
    {
        SetScore(0);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        pacman.gameObject.SetActive(true);
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    /*    private void GameOver()
        {
            gameOverText.enabled = true;

            pacman.gameObject.SetActive(false);

            Debug.Log("Game Over called. Submitting score: " + this.score);

            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].gameObject.SetActive(false);
            }

            // Ensure this line is not redundant (it's in your code twice, but won't hurt)

            if (LeaderboardDisplay.instance != null)
            {
                LeaderboardDisplay.instance.AddToLeaderboard(this.score);
            }

        }
    */

    private IEnumerator GameOverSequence()
    {
        const float gameOverDuration = 6f;

        if (LeaderboardDisplay.instance != null)
        {
            LeaderboardDisplay.instance.AddToLeaderboard(this.score);
        }

        pacman.gameObject.SetActive(false);

        gameOverText.enabled = true;

        Debug.Log($"Game Over! Showing text for {gameOverDuration} seconds.");
        yield return new WaitForSeconds(gameOverDuration);

        gameOverText.enabled = false;

        ScreenManager screenManager = FindObjectOfType<ScreenManager>();
        if (screenManager != null)
        {
            Debug.Log("Transitioning back to Start Screen...");
            screenManager.TransitionToStartScreen();
        }
        else
        {
            Debug.LogError("ScreenManager not found for transition.");
        }
    }
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();
        StartCoroutine(GameOverSequence());
        // SetScore(0); 

    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            //Invoke(nameof(GameOver), 3f);
            StartCoroutine(GameOverSequence());
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        //int count = 0;
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        //Debug.Log($"Found {count} active pellets.");
        //return count > 0;
        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

}