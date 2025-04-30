using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    public void HeroWins()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void HeroLoses()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
