using UnityEngine;

public class BallInteraction : MonoBehaviour
{
    public Animator animator;

    [Header("Goals")]
    public Transform rGoal;
    public Transform bGoal;

    [Header("Colliders")]
    public Collider punchCollider1;
    public Collider punchCollider2;
    public Collider kickCollider;
    public Collider headerCollider;

    public Rigidbody ballRigidbody;

    // Trigger Flags
    private bool isPunching = false;
    private bool isKicking = false;
    private bool isHeading = false;

    public bool GoalB()
    {
        // Check if the ball is past the red or blue goals
        return ballRigidbody.position.x < bGoal.position.x;
    }

    public bool GoalR()
    {
        // Check if the ball is past the red or blue goals
        return ballRigidbody.position.x > rGoal.position.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check which body part triggered the collision
        if (other == headerCollider & isHeading)
        {
            ApplyForce(transform.forward - transform.up, 15f); // Header force
        }
        else if (other == punchCollider1 && isPunching || other == punchCollider2 && isPunching)
        {
            ApplyForce(transform.forward, 15f); // Punch force
        }
        else if (other == kickCollider & isKicking)
        {
            ApplyForce(transform.forward + transform.up, 15f); // Kick force
        }
    }

    public void ResetBall()
    {
        // Stop the ball's movement and rotation
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    // Apply force with a specific direction and strength
    private void ApplyForce(Vector3 forceDirection, float forceStrength)
    {
        // Apply the force to the ball
        ballRigidbody.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
    }

    public void Punch()
    {
        isPunching = true;

        // Trigger punch animation
        animator.SetBool("punch", true);

        // Reset punch animation and flag after a delay
        Invoke("ResetPunch", 0.417f);
    }

    public void Kick()
    {
        isKicking = true;

        // Trigger kick animation
        animator.SetBool("kick", true);

        // Reset kick animation and flag after a delay
        Invoke("ResetKick", 0.417f);
    }

    public void Header()
    {
        isHeading = true;

        // Trigger header animation
        animator.SetBool("header", true);

        // Reset header animation and flag after a delay
        Invoke("ResetHeader", 0.417f);
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
