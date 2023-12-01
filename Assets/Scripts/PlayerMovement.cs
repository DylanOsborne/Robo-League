using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float airMultiplier;
    public bool secondJump;

    public float customGravity = -20f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode punchKey = KeyCode.Mouse0;
    public KeyCode kickKey = KeyCode.Mouse1;
    public KeyCode headerKey = KeyCode.Mouse2;
    public KeyCode walkKeyW = KeyCode.W;
    public KeyCode walkKeyA = KeyCode.A;
    public KeyCode walkKeyS = KeyCode.S;
    public KeyCode walkKeyD = KeyCode.D;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool grounded;

    [Header("Misc")]

    public Transform orientation;

    float horzontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Rigidbody ballRigidbody;

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
        horzontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {

        moveDirection = orientation.forward * verticalInput + orientation.right * horzontalInput;

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
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        animator.SetTrigger("jump");
    }

    private void Punch()
    {
        animator.SetBool("punch", true);

        ApplyPunchForce();

        Invoke("ResetPunch", 0.417f);
    }

    private void Kick()
    {
        animator.SetBool("kick", true);

        ApplyKickForce();

        Invoke("ResetKick", 0.417f);
    }

    private void Header()
    {
        animator.SetBool("header", true);

        ApplyHeaderForce();

        Invoke("ResetHeader", 0.417f);
    }

    private void ApplyPunchForce()
    {
        if (ballRigidbody != null)
        {
            Vector3 punchForceDirection = transform.forward;
            float punchForceStrength = 10f;

            ballRigidbody.AddForce(punchForceDirection * punchForceStrength, ForceMode.Impulse);
        }
    }

    private void ApplyKickForce()
    {
        if (ballRigidbody != null)
        {
            Vector3 kickForceDirection = transform.forward + transform.up;
            float kickForceStrength = 10f;

            ballRigidbody.AddForce(kickForceDirection * kickForceStrength, ForceMode.Impulse);
        }
    }

    private void ApplyHeaderForce()
    {
        if (ballRigidbody != null)
        {
            Vector3 headerForceDirection = transform.forward - transform.up;
            float headerForceStrength = 10f;

            ballRigidbody.AddForce(headerForceDirection * headerForceStrength, ForceMode.Impulse);
        }
    }


    // Methods to reset the action parameters
    private void ResetPunch(){ animator.SetBool("punch", false); }
    private void ResetKick() { animator.SetBool("kick", false); }
    private void ResetHeader() { animator.SetBool("header", false); }

}
