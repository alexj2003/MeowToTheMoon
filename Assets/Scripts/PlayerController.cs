using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    float jumpSpeed = 8.0f;
    float gravity = 9.8f * 16;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update vertical speed
        float vertical = 0.0f;
        if (Keyboard.current.spaceKey.isPressed) {
            vertical = jumpSpeed;
        }

        // Apply gravity
        // vertical -= gravity * Time.deltaTime;

        // Move the player
        Vector3 move = new Vector3(0, vertical, 0);
        transform.Translate(move * Time.deltaTime);
    }
}
