using UnityEngine;
using UnityEngine.SceneManagement;

public class MarvinController : MonoBehaviour
{
    // When player collides with this sprite, switch to the closing scene
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            // Switch scene
            SceneManager.LoadScene("Closing Scene");
        }
    }
}
