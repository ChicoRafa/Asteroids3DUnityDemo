using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is responsible for controlling the main menu.
 */
public class MenuController : MonoBehaviour
{
    public void LoadGameLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
