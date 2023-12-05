using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts")]
    public StaminaSystem staminaSystem;
    public InputManager inputManager;
    public BallInteraction ballInteraction;

    [Header("Movement")]
    public float groundDrag = 1f;  // Drag applied when the player is on the ground.

    public float jumpForce = 25f;  // Force applied when jumping.
    private float airMultiplier = 2f;  // Multiplier for movement in the air.
    public bool secondJump;  // Flag for allowing a second jump.

    public float customGravity = -20f;  // Custom gravity to simulate realistic falling.

    float moveSpeed;  // Speed of the player's movement.

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

    public Rigidbody rb;  // Reference to the player's Rigidbody component.

    public Animator animator;  // Reference to the player's Animator component.

    public bool isPaused = false;

    private void Start()
    {
        rb.freezeRotation = true;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    private void Update()
    {
        if(!isPaused)
        {
            moveSpeed = staminaSystem.moveSpeed;

            grounded = CheckGrounded();
            SetInput();
            SpeedControl();

            animator.SetBool("grounded", grounded);

            if (grounded)
            {
                secondJump = true;

                rb.drag = groundDrag;

                if (Input.GetKeyDown(inputManager.jumpKey))
                {
                    Jump();
                }

                if (Input.GetKey(inputManager.walkKeyW) || Input.GetKey(inputManager.walkKeyA) || Input.GetKey(inputManager.walkKeyS) || Input.GetKey(inputManager.walkKeyD))
                {
                    // Check if sprint key is pressed and there's enough stamina
                    if (Input.GetKey(inputManager.sprintKey))
                    {
                        staminaSystem.Sprint();
                    }
                    else
                    {
                        staminaSystem.RechargeStamina();
                    }

                    animator.SetBool("walking", true);
                }
                else
                {
                    staminaSystem.RechargeStamina();

                    animator.SetBool("walking", false);
                }
            }
            else if (!grounded)
            {
                staminaSystem.RechargeStamina();

                rb.drag = 0;

                if (Input.GetKeyDown(inputManager.jumpKey) && secondJump == true)
                {
                    secondJump = false;

                    Jump();
                }
            }

            if (Input.GetKeyDown(inputManager.punchKey)) { ballInteraction.Punch(); }

            if (Input.GetKeyDown(inputManager.kickKey)) { ballInteraction.Kick(); }

            if (Input.GetKeyDown(inputManager.headerKey)) { ballInteraction.Header(); }
        }
    }

    private void FixedUpdate()
    {
        if(!isPaused)
        {
            MovePlayer();

            rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
        }
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
}