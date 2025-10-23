using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public static bool IsGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Public function to resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        
        Time.timeScale = 1f;
        
        IsGamePaused = false;
    }

    // Function to pause the game
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        
        IsGamePaused = true;
    }

    //  Quit Button 
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
    
    }
}