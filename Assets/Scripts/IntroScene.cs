using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public void ContinueGame() {
        SceneManager.LoadScene("Apartment");
    }
}
