using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame method called.");
        SceneManager.LoadScene("Level1");
    }

    public void OpenOptions()
    {
        Debug.Log("OpenOptions method called.");
        SceneManager.LoadScene("ControlsScene");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame method called.");
        Application.Quit();
    }
}
