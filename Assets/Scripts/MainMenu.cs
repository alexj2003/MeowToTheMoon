using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start the game
    public void PlayGame() {
        SceneManager.LoadScene("Intro Scene");
    }

    // End the game, quit the application
    public void EndGame() {
        Application.Quit();
    }
}
