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
    public float groundDrag = 1f;

    public float jumpForce = 25f;
    private float airMultiplier = 2f; 
    public bool secondJump;

    public float customGravity = -20f;  

    float moveSpeed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool grounded;

    [Header("Misc")]
    public Transform orientation; 

    float horizontalInput; 
    float verticalInput;  

    Vector3 moveDirection; 

    public Rigidbody rb;

    public Animator animator;

    public bool isPaused = false;

    private void Start()
    {
        // Freeze rotation on start
        rb.freezeRotation = true;
    }

    public void SetPaused(bool paused)
    {
        // Set pause state
        isPaused = paused;
    }

    private void Update()
    {
        if(!isPaused)
        {
            // Update move speed and check for inputs
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
            // Move player and apply custom gravity
            MovePlayer();

            rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
        }
    }

    private void SetInput()
    {
        // Get input values
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            // Move player on the ground
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
        else
        {
            // Apply air multiplier if in the air
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
        // Reset y velocity and apply upward force for jumping
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Trigger jump animation
        animator.SetTrigger("jump");
    }
}