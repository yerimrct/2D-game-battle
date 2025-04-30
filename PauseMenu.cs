using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; 

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        GameIsPaused = false;
    }

    public void LoadSettings()
    {
        Debug.Log("Open Settings (optional)");
        
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reset timeScale before loading menu
        SceneManager.LoadScene("MainMenu"); 
    }
}
