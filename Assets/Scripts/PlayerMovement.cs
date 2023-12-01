using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;  // Speed of the player's movement.
    public float groundDrag;  // Drag applied when the player is on the ground.

    public float jumpForce;  // Force applied when jumping.
    public float airMultiplier;  // Multiplier for movement in the air.
    public bool secondJump;  // Flag for allowing a second jump.

    public float customGravity = -20f;  // Custom gravity to simulate realistic falling.

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;  // Key to trigger a jump.
    public KeyCode punchKey = KeyCode.Mouse0;  // Key to trigger a punch.
    public KeyCode kickKey = KeyCode.Mouse1;  // Key to trigger a kick.
    public KeyCode headerKey = KeyCode.Mouse2;  // Key to trigger a header.
    public KeyCode walkKeyW = KeyCode.W;  // Key for forward movement.
    public KeyCode walkKeyA = KeyCode.A;  // Key for left movement.
    public KeyCode walkKeyS = KeyCode.S;  // Key for backward movement.
    public KeyCode walkKeyD = KeyCode.D;  // Key for right movement.

    [Header("Ground Check")]
    public Transform groundCheck;  // Transform representing the ground check position.
    public float groundCheckRadius = 0.2f;  // Radius for the ground check sphere.
    public LayerMask groundLayer;  // Layer mask for identifying the ground.
    public bool grounded;  // Flag indicating whether the player is grounded.

    [Header("Misc")]
    public Transform orientation;  // Transform representing the player's orientation.

    float horizontalInput;  // Input for horizontal movement.
    float verticalInput;  // Input for vertical movement.

    Vector3 moveDirection;  // Calculated movement direction.

    Rigidbody rb;  // Reference to the player's Rigidbody component.

    public Rigidbody ballRigidbody;  // Reference to the Rigidbody of the ball.

    public Animator animator;  // Reference to the player's Animator component.

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        grounded = CheckGrounded();
        SetInput();
        SpeedControl();

        animator.SetBool("grounded", grounded);

        if (grounded)
        {
            secondJump = true;

            rb.drag = groundDrag;

            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }

            if (Input.GetKey(walkKeyW) || Input.GetKey(walkKeyA) || Input.GetKey(walkKeyS) || Input.GetKey(walkKeyD))
            {
                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);
            }
        }
        else if (!grounded)
        {
            rb.drag = 0;

            if (Input.GetKeyDown(jumpKey) && secondJump == true)
            {
                secondJump = false;

                Jump();
            }
        }

        if (Input.GetKeyDown(punchKey))
        {
            Punch();
        }

        if (Input.GetKeyDown(kickKey))
        {
            Kick();
        }

        if (Input.GetKeyDown(headerKey))
        {
            Header();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
    }

    private void SetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
        else
        {
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
    }

    private bool CheckGrounded()
    {
        // Use a sphere cast to check if the player is grounded
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply upward force for jumping
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Trigger jump animation
        animator.SetTrigger("jump");
    }

    private void Punch()
    {
        // Trigger punch animation
        animator.SetBool("punch", true);

        // Apply force to the ball
        ApplyPunchForce();

        // Reset punch animation after a delay
        Invoke("ResetPunch", 0.417f);
    }

    private void Kick()
    {
        // Trigger kick animation
        animator.SetBool("kick", true);

        // Apply force to the ball
        ApplyKickForce();

        // Reset kick animation after a delay
        Invoke("ResetKick", 0.417f);
    }

    private void Header()
    {
        // Trigger header animation
        animator.SetBool("header", true);

        // Apply force to the ball
        ApplyHeaderForce();

        // Reset header animation after a delay
        Invoke("ResetHeader", 0.417f);
    }

    private void ApplyPunchForce()
    {
        // Check if the ball's Rigidbody is available
        if (ballRigidbody != null)
        {
            // Calculate punch force direction and strength
            Vector3 punchForceDirection = transform.forward;
            float punchForceStrength = 10f;

            // Apply the punch force to the ball
            ballRigidbody.AddForce(punchForceDirection * punchForceStrength, ForceMode.Impulse);
        }
    }

    private void ApplyKickForce()
    {
        // Check if the ball's Rigidbody is available
        if (ballRigidbody != null)
        {
            // Calculate kick force direction and strength
            Vector3 kickForceDirection = transform.forward + transform.up;
            float kickForceStrength = 10f;

            // Apply the kick force to the ball
            ballRigidbody.AddForce(kickForceDirection * kickForceStrength, ForceMode.Impulse);
        }
    }

    private void ApplyHeaderForce()
    {
        // Check if the ball's Rigidbody is available
        if (ballRigidbody != null)
        {
            // Calculate header force direction and strength
            Vector3 headerForceDirection = transform.forward - transform.up;
            float headerForceStrength = 10f;

            // Apply the header force to the ball
            ballRigidbody.AddForce(headerForceDirection * headerForceStrength, ForceMode.Impulse);
        }
    }

    // Methods to reset the action parameters
    private void ResetPunch() { animator.SetBool("punch", false); }
    private void ResetKick() { animator.SetBool("kick", false); }
    private void ResetHeader() { animator.SetBool("header", false); }
}