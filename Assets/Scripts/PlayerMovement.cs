using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 60f;  // Speed of the player's movement.
    public float groundDrag = 1f;  // Drag applied when the player is on the ground.

    public float jumpForce = 25f;  // Force applied when jumping.
    private float airMultiplier = 2f;  // Multiplier for movement in the air.
    public bool secondJump;  // Flag for allowing a second jump.

    public float customGravity = -20f;  // Custom gravity to simulate realistic falling.

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float sprintStaminaConsumptionRate = 35f;
    public float staminaRechargeRate = 20f;
    public float currentStamina;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;  // Key to trigger a jump.
    public KeyCode punchKey = KeyCode.Mouse0;  // Key to trigger a punch.
    public KeyCode kickKey = KeyCode.Mouse1;  // Key to trigger a kick.
    public KeyCode headerKey = KeyCode.Mouse2;  // Key to trigger a header.
    public KeyCode walkKeyW = KeyCode.W;  // Key for forward movement.
    public KeyCode walkKeyA = KeyCode.A;  // Key for left movement.
    public KeyCode walkKeyS = KeyCode.S;  // Key for backward movement.
    public KeyCode walkKeyD = KeyCode.D;  // Key for right movement.
    public KeyCode sprintKey = KeyCode.LeftShift; // Key to activate sprinting

    [Header("Ground Check")]
    public Transform groundCheck;  // Transform representing the ground check position.
    public float groundCheckRadius = 0.2f;  // Radius for the ground check sphere.
    public LayerMask groundLayer;  // Layer mask for identifying the ground.
    public bool grounded;  // Flag indicating whether the player is grounded.

    [Header("Ball Interaction")]
    public Collider punchCollider1; // Attach a trigger collider for left punch detection
    public Collider punchCollider2; // Attach a trigger collider for right punch detection
    public Collider kickCollider;  // Attach a trigger collider for kick detection
    public Collider headerCollider; // Attach a trigger collider for header detection

    // Flags for action states
    private bool isPunching;
    private bool isKicking;
    private bool isHeading;

    [Header("SpawnPositions")]
    public Transform ballSpawmPos;
    public Transform playerSpawnPos1;
    public Transform playerSpawnPos2;
    public Transform playerSpawnPos3;
    public Transform playerSpawnPos4;
    public Transform playerSpawnPos5;
    public Transform playerSpawnPos6;

    [Header("Misc")]
    public Transform orientation;  // Transform representing the player's orientation.

    float horizontalInput;  // Input for horizontal movement.
    float verticalInput;  // Input for vertical movement.

    Vector3 moveDirection;  // Calculated movement direction.

    Rigidbody rb;  // Reference to the player's Rigidbody component.

    public Rigidbody ballRigidbody;  // Reference to the Rigidbody of the ball.

    public Animator animator;  // Reference to the player's Animator component.

    [SerializeField] private TextMeshProUGUI staminaText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;

        ballRigidbody.position = ballSpawmPos.position;
        rb.position = playerSpawnPos1.position;
        rb.rotation = playerSpawnPos1.rotation;

        currentStamina = maxStamina;

        // Set the X position of the text
        Vector3 newTextWidth = staminaText.transform.position;
        newTextWidth.x = Screen.width * 0.95f;
        staminaText.transform.position = newTextWidth;

        // Set the y position of the text
        Vector3 newTextHeight = staminaText.transform.position;
        newTextHeight.y = Screen.height * 0.3f;
        staminaText.transform.position = newTextHeight;
    }

    private void Update()
    {
        staminaText.text = Mathf.RoundToInt(currentStamina).ToString();

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
                // Check if sprint key is pressed and there's enough stamina
                if (Input.GetKey(sprintKey) && currentStamina > 0)
                {
                    // Sprinting
                    moveSpeed = 110f;
                    animator.SetFloat("walkSpeed", 1.5f);

                    // Consume stamina
                    currentStamina -= sprintStaminaConsumptionRate * Time.deltaTime;

                    // Disable sprinting if stamina is zero
                    if (currentStamina <= 0)
                    {
                        currentStamina = 0;
                        moveSpeed = 60f;
                        animator.SetFloat("walkSpeed", 1f);
                    }
                }
                else
                {
                    // Not sprinting, gradually recharge stamina
                    currentStamina += staminaRechargeRate * Time.deltaTime;

                    // Clamp the stamina value to the maximum
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

                    // Regular walking speed
                    moveSpeed = 60f;
                    animator.SetFloat("walkSpeed", 1f);
                }

                animator.SetBool("walking", true);
            }
            else
            {
                // Not sprinting, gradually recharge stamina
                currentStamina += staminaRechargeRate * Time.deltaTime;

                // Clamp the stamina value to the maximum
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

                animator.SetBool("walking", false);
            }
        }
        else if (!grounded)
        {
            moveSpeed = 60f;

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
        // Set punch flag to true
        isPunching = true;

        // Trigger punch animation
        animator.SetBool("punch", true);

        // Reset punch animation and flag after a delay
        Invoke("ResetPunch", 0.417f);
    }

    private void Kick()
    {
        // Set kick flag to true
        isKicking = true;

        // Trigger kick animation
        animator.SetBool("kick", true);

        // Reset kick animation and flag after a delay
        Invoke("ResetKick", 0.417f);
    }

    private void Header()
    {
        // Set header flag to true
        isHeading = true;

        // Trigger header animation
        animator.SetBool("header", true);

        // Reset header animation and flag after a delay
        Invoke("ResetHeader", 0.417f);
    }

    private void ApplyForce(Collider collider, Vector3 forceDirection, float forceStrength)
    {
        if (collider != null && ballRigidbody != null)
        {
            // Check if the collider is in contact with the ball and the corresponding action flag is true
            if (collider.bounds.Intersects(ballRigidbody.GetComponent<Collider>().bounds) &&
                collider == punchCollider1 && isPunching ||
                collider == punchCollider2 && isPunching ||
                collider == kickCollider && isKicking ||
                collider == headerCollider && isHeading)
            {
                // Apply the force to the ball
                ballRigidbody.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Apply forces when the trigger colliders are in contact with the ball
        ApplyForce(punchCollider1, transform.forward, 5f);
        ApplyForce(punchCollider2, transform.forward, 5f);
        ApplyForce(kickCollider, transform.forward + transform.up, 5f);
        ApplyForce(headerCollider, transform.forward - transform.up, 5f);
    }

    // Methods to reset the action parameters
    private void ResetPunch()
    {
        animator.SetBool("punch", false);
        isPunching = false;
    }

    private void ResetKick()
    {
        animator.SetBool("kick", false);
        isKicking = false;
    }

    private void ResetHeader()
    {
        animator.SetBool("header", false);
        isHeading = false;
    }
}