using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/**
 * This class is responsible for controlling the pause menu functionality.
 * Manages pause state, UI display, and menu interactions.
 */
public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    
    [Header("Game State")]
    private bool isPaused = false;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        
        if (pauseMenuPanel)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
    
    public void OnPause(InputValue inputValue)
    {
        if (gameManager && gameManager.currentState == GameState.Playing)
        {
            TogglePause();
        }
        else if (isPaused)
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuPanel)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        
        if (pauseMenuPanel)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
 
    public bool IsPaused => isPaused;
}