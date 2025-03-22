using UnityEngine;
using UnityEngine.InputSystem;

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

    // Values for vertical movement
    public const float minJumpForce = 2.0f;
    public const float maxJumpForce = 20.0f;
    public const float chargeRate = 20.0f;
    public float jumpCharge = 0.0f;

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
                    } else {
                        // If we're touching a wall, add a small bounce force to prevent getting stuck
                        rb.AddForce(new Vector2(bounceForce * moveDirection, 0.0f), ForceMode2D.Impulse);
                    }
                }
                break;
        }
    }

    // Check if an object is grounded
    private bool IsGrounded() {
        // Check velocity and if the player is touching the ground
        return Mathf.Abs(rb.linearVelocity.y) < 0.1f && Physics2D.OverlapBox(
            new Vector2(gameObject.transform.position.x, 
            gameObject.transform.position.y - 0.5f), 
            new Vector2(0.9f, 0.4f), 0f, groundMask);
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
                if (Mathf.Abs(c.normal.x) > 0.5f) {
                    // Reverse horizontal movement
                    moveDirection *= -1;
                    UpdateSpriteDirection();

                    // Add a small bounce force to prevent getting stuck in walls
                    rb.AddForce(new Vector2(bounceForce * c.normal.x, 0.0f), ForceMode2D.Impulse);

                    break;
                }
            }
        }
    }

    // Update the sprite direction
    private void UpdateSpriteDirection() {
        sr.flipX = moveDirection == -1;
    }
}
