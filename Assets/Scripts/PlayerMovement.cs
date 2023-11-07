using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool secondJump;

    public float customGravity = -20f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool grounded;

    public Transform orientation;

    float horzontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = CheckGrounded();
        SetInput();
        SpeedControl();

        if (grounded)
        {
            secondJump = true;

            rb.drag = groundDrag;

            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }
        } 
        else if(!grounded)
        {
            rb.drag = 0;

            if (Input.GetKeyDown(jumpKey) && secondJump == true)
            {
                secondJump = false;

                Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        // Apply custom gravity to the character
        rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
    }

    private void SetInput()
    {
        horzontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horzontalInput;

        // on ground
        if(grounded)
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
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
