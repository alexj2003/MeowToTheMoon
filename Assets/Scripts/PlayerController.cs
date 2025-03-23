using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum PlayerState {
    Idle,
    Charging,
    Jumping
}

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public PlayerState state = PlayerState.Idle;
    public LayerMask groundMask;
    public Camera camera;
    public AudioSource[] audioSources;
    public AudioSource audioClip;
    public int i = 0;

    // Minimum camera Y position
    public const float minCameraY = 0f;

    // Values for vertical movement
    public const float minJumpForce = 2.0f;
    public const float maxJumpForce = 12.0f;
    public const float chargeRate = 20.0f;
    public float jumpCharge = 0.0f;
    public float speed = 0.0f;

    // Values for horizontal movement
    public const float bounceForce = 2.0f;
    public int moveDirection = 1; // 1 for right, -1 for left

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }

        if (sr == null) {
            sr = GetComponent<SpriteRenderer>();
        }

        if (camera == null) {
            camera = Camera.main;
        }

        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input class isn't recommended for new projects but this is the simplest way for now
        switch (state) {
            case PlayerState.Idle:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    // Start charging the jump
                    state = PlayerState.Charging;
                    jumpCharge = 0.0f;
                }
                break;
            case PlayerState.Charging:
                if (Keyboard.current.spaceKey.isPressed) {
                    // Charge the jump if the space key is held
                    jumpCharge += chargeRate * Time.deltaTime;
                    jumpCharge = Mathf.Clamp(jumpCharge, 0.0f, maxJumpForce);
                }
                if (Input.GetKeyUp(KeyCode.Space)) {
                    // Jump when the space key is released
                    float jumpForce = Mathf.Max(minJumpForce, jumpCharge);
                    
                    // Reset the vertical velocity and apply the jump + horizontal forces
                    rb.linearVelocity = new Vector2(jumpCharge / 2 * moveDirection, 0.0f);
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    speed = Mathf.Sqrt(rb.linearVelocity.x * rb.linearVelocity.x + rb.linearVelocity.y * rb.linearVelocity.y);

                    state = PlayerState.Jumping;
                    jumpCharge = 0.0f;
                }
                break;
            case PlayerState.Jumping:
                // Only stop movement if grounded and no side collision - prevents getting stuck in corners
                if (IsGrounded()) {
                    if (!IsTouchingWall()) {
                        // If we're not touching a wall, stop horizontal movement
                        rb.linearVelocity = new Vector2(0.0f, 0.0f);
                        state = PlayerState.Idle;

                        speed = Mathf.Sqrt(rb.linearVelocity.x * rb.linearVelocity.x + rb.linearVelocity.y * rb.linearVelocity.y);
                        //Debug.Log("Landed");
                    } else {
                        // this doesnt run
                        // If we're touching a wall, add a small bounce force to prevent getting stuck
                        float bounceAmount = bounceForce * moveDirection;
                        rb.AddForce(new Vector2(bounceAmount, 0.0f), ForceMode2D.Impulse);
                        
                        Debug.Log("Bounce in not run section");
                        
                    }
                }
                break;
        }

        UpdateCameraPosition();
        // Debug.Log("state = " + state);
    }

    // Update the camera's y position
    void UpdateCameraPosition() {
        // New Y position - player's Y but not lower than minimum
        float targetY = Mathf.Max(minCameraY, transform.position.y);
        //Debug.Log("Target Y: " + targetY);
        camera.transform.position = new Vector3(0, targetY, -10);
    }

    // Check if an object is grounded
    private bool IsGrounded() {
        // Check velocity and if the player is touching the ground
        return Mathf.Abs(rb.linearVelocity.y) < 0.1f && Physics2D.OverlapBox(
            new Vector2(gameObject.transform.position.x, 
            gameObject.transform.position.y - 0.5f), 
            new Vector2(1.4f, 0.4f), 0f, groundMask);
    }

    // Check if an object is touching a horizontal wall
    private bool IsTouchingWall() {
        return Physics2D.OverlapBox(
            new Vector2(transform.position.x + moveDirection * 0.5f, transform.position.y),
            new Vector2(0.1f, 0.9f), 0f, groundMask
        );
    }

    // Wall collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground Collisions")) {
            // Check if collision is horizontal
            foreach (ContactPoint2D c in collision.contacts) {
                if (Mathf.Abs(c.normal.x) > 0.5f) { // c.normal.x = 1 or -1 (or close enough), rest of the time is 0 (or close enough)
                    // Reverse horizontal movement
                    moveDirection *= -1;
                    //Debug.Log("Wall collision, direction: " + moveDirection);
                    UpdateSpriteDirection();

                    //Debug.Log("Speed/10 = " + speed/10);

                    // Add a small bounce force to prevent getting stuck in walls
                    // removed  * 
                    rb.AddForce(new Vector2((speed/10) * c.normal.x * bounceForce, 0.0f), ForceMode2D.Impulse);

                    i = Random.Range(0, audioSources.Length);
                    Debug.Log("audioSources.Length, i = " + audioSources.Length + ", " + i);
                    
                    audioClip = audioSources[i];
                    audioClip.Play();

                    break;
                }
            }
        }
    }

    // Update the sprite direction
    private void UpdateSpriteDirection() {
        sr.flipX = moveDirection == -1;
    }

    // Switch to the next scene
    private void SwitchNextScene() {
        // List of scenes
        string[] scenes = {
            "Apartment",
            "Building",
            "Radio Tower",
            "Sky",
            "Space",
            "Moon"
        };

        // Get the current scene index
        int currentSceneIndex = Array.IndexOf(scenes, SceneManager.GetActiveScene().name);

        // Load the next scene
        if (currentSceneIndex < scenes.Length - 1) {
            SceneManager.LoadScene(scenes[currentSceneIndex + 1]);
        }
    }
}
