using UnityEngine;
using UnityEngine.SceneManagement;

// Used by the MENU button in the black hole scene in ordeer to head back to the main menu.
public class BackToMenu : MonoBehaviour
{
    public void GoToMenu()
    {
        
        SceneManager.LoadScene("MainMenu");
    }
}