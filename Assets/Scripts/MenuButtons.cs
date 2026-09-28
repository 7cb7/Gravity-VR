using UnityEngine;
using UnityEngine.SceneManagement;

// Said main menu buttons , each one just loads its scene.
// *Note to self ~ avoide previous mistakes :Scene names have to match Build Profiles exactly or nothing happens, 
public class MenuButtons : MonoBehaviour
{
    public void LoadBlackHole()   { SceneManager.LoadScene("BlackHole"); }
    public void LoadMoon()        { SceneManager.LoadScene("MoonGravity"); }
    public void LoadSolarSystem() { SceneManager.LoadScene("SolarSystem"); }
    public void LoadQuiz()        { SceneManager.LoadScene("Quiz"); }

    // closes the app on the headset (does nothing in the Editor, that's normal)
    public void QuitApp() { Application.Quit(); }
}