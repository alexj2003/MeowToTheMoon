using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start the game
    public void PlayGame() {
        Debug.Log("Playing game");
        SceneManager.LoadScene("Intro Scene");
    }

    // End the game, quit the application
    public void EndGame() {
        Debug.Log("Ending game");
        Application.Quit();
    }
}
