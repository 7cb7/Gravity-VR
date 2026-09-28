using UnityEngine;
using UnityEngine.SceneManagement;

// this is same idea as BackToMenu, used by the MENU buttons in the other scenes.
public class GoToMainMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
    
        SceneManager.LoadScene("MainMenu");
    }
} 