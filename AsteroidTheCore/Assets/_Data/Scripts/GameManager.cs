using System.Collections;
using TMPro;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver,
    LevelFinished,
    Paused
}
public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public GameState currentState = GameState.Playing;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelFinishedPanel;
    [Header("Player Lives")]
    [SerializeField] private int lives = 3;
    public TMP_Text livesText;
    [Header("Remaining Asteroids")]
    [SerializeField] private int remainingAsteroids;
    private PauseMenuManager pauseMenuManager;
    
    

    public int Lives
    {
        get => lives;
        set
        {
           lives = value; 
           livesText.text = "x" + lives;
        } 
    }
    
    public int RemainingAsteroids
    {
        get => remainingAsteroids;
        set
        {
            remainingAsteroids = value;
            if (remainingAsteroids <= 0)
            {
                CheckEndGame();
            }
        }
    }

    private void Start()
    {
        pauseMenuManager = FindFirstObjectByType<PauseMenuManager>();
        SetState(currentState);
        gameOverPanel.SetActive(false);
        levelFinishedPanel.SetActive(false);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Playing:
                if (!pauseMenuManager || !pauseMenuManager.IsPaused)
                {
                    Time.timeScale = 1f;
                }
                break;
            case GameState.LevelFinished:
                levelFinishedPanel.SetActive(true);
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                gameOverPanel.SetActive(true);
               Time.timeScale = 0f;
                break;
        }
    }
    
    public void CheckEndGame()
    {
        if (lives <= 0)
        {
            SetState(GameState.GameOver);
        }
        else if (remainingAsteroids <= 0)
        {
            SetState(GameState.LevelFinished);
        }
    }
}