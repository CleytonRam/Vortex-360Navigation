using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    
    public void StartGame()
    {
        
        SceneManager.LoadScene("PanoramaView");
    }

    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo.");
    }
}