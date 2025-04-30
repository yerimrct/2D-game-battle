using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;    
    public GameObject settingsPanel; 
    public AudioSource backgroundMusic; 

    private bool isMuted = false;

    private void Start()
    {
        Time.timeScale = 0f;  // Pause the game at the start
        mainMenuUI.SetActive(true); // Show the main menu
        settingsPanel.SetActive(false); // Hide settings panel
    }

    public void StartGame()
    {
        Time.timeScale = 1f;  // Resume the game
        mainMenuUI.SetActive(false); // Hide the main menu
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    public void QuitGame()
{
    Debug.Log("Quitting game...");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
}


    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ToggleMute()
    {
        if (backgroundMusic != null)
        {
            isMuted = !isMuted;
            backgroundMusic.mute = isMuted;
        }
    }
}
