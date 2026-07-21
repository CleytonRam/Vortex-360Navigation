using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel; 

    private bool isPaused = false;

    void Start()
    {
        
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; 
        if (pausePanel != null)
            pausePanel.SetActive(true);

        
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; 
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void ResumeFromButton()
    {
        ResumeGame();
    }

    
    public void GoToMainMenu()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}