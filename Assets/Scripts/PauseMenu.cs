using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Is game paused
    bool isPaused;

    // Pause menu
    public GameObject pausePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pausePanel == null) {
            pausePanel = GameObject.Find("PausePanel");
        }
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // Toggle pause
            isPaused = !isPaused;
        }

        CheckPause();
    }

    // Update time scale and visibility of pause panel
    private void CheckPause() {
        Time.timeScale = isPaused ? 0 : 1;
        pausePanel.SetActive(isPaused);
    }

    public void MainMenu() {
        // Load main menu scene
        isPaused = false;
        CheckPause();
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart() {
        // Reload current scene
        isPaused = false;
        CheckPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        // Quit the application
        Application.Quit();
    }
}
