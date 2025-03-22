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
    public float minJumpForce = 5.0f;
    public float maxJumpForce = 10.0f;
    public float chargeRate = 10.0f;
    public float jumpCharge = 0.0f;
    public PlayerState state = PlayerState.Idle;
    float distToGround;
    public LayerMask groundMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
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
                    
                    // Reset the vertical velocity and apply the jump force
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0.0f);
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                    state = PlayerState.Jumping;
                    jumpCharge = 0.0f;
                }
                break;
            case PlayerState.Jumping:
                if (IsGrounded()) {
                    // Change state back to idle when the player lands
                    state = PlayerState.Idle;
                }
                break;
        }
    }

    // Check if an object is grounded
    private bool IsGrounded() {
        // Check velocity and if the player is touching the ground
        if (rb.linearVelocity.y == 0 && Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), new Vector2(0.9f, 0.4f), 0f, groundMask)) {
            return true;
        }

        return false;
    }
}
