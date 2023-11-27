using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Body Parts")]
    public Transform head;
    public Transform hands;
    public Transform feet;

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
    public KeyCode punchKey = KeyCode.Mouse0;
    public KeyCode kickKey = KeyCode.Mouse1;
    public KeyCode headerKey = KeyCode.Mouse2;
    public KeyCode walkKey = KeyCode.W;

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

    public Animator animator;

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

        if (grounded)
        {
            secondJump = true;

            rb.drag = groundDrag;

            if (Input.GetKeyDown(jumpKey))
            {
                Jump();
            }

            if (Input.GetKey(walkKey))
            {
                animator.SetBool("walking", true);
            } 
            else
            {
                animator.SetBool("walking", false);
            }

            animator.SetBool("jump", false);
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

        animator.SetBool("jump", true);
    }

    private void Punch()
    {
        animator.SetBool("punch", true);

        Invoke("ResetPunch", 0.417f);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Kick()
    {
        animator.SetBool("kick", true);

        Invoke("ResetKick", 0.417f);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Header()
    {
        animator.SetBool("header", true);

        Invoke("ResetHeader", 0.417f);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    // Methods to reset the action parameters
    private void ResetPunch(){ animator.SetBool("punch", false); }
    private void ResetKick() { animator.SetBool("kick", false); }
    private void ResetHeader() { animator.SetBool("header", false); }

}
