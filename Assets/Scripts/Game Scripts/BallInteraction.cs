using UnityEngine;

public class BallInteraction : MonoBehaviour
{
    public Animator animator;

    [Header("Goals")]
    public Transform rGoal;
    public Transform bGoal;

    [Header("Colliders")]
    public Collider punchCollider1; // Attach a trigger collider for left punch detection
    public Collider punchCollider2; // Attach a trigger collider for right punch detection
    public Collider kickCollider;  // Attach a trigger collider for kick detection
    public Collider headerCollider; // Attach a trigger collider for header detection

    public Rigidbody ballRigidbody;  // Reference to the Rigidbody of the ball.

    // Flags for action states
    private bool isPunching;
    private bool isKicking;
    private bool isHeading;

    public bool Goal()
    {
        return ballRigidbody.position.x > rGoal.position.x || ballRigidbody.position.x < bGoal.position.x;
    }

    public void ResetBall()
    {
        // Stop the ball's movement
        ballRigidbody.velocity = Vector3.zero;

        // Stop the ball's rotation
        ballRigidbody.angularVelocity = Vector3.zero;
    }

    public void Punch()
    {
        // Set punch flag to true
        isPunching = true;

        // Trigger punch animation
        animator.SetBool("punch", true);

        // Reset punch animation and flag after a delay
        Invoke("ResetPunch", 0.417f);
    }

    public void Kick()
    {
        // Set kick flag to true
        isKicking = true;

        // Trigger kick animation
        animator.SetBool("kick", true);

        // Reset kick animation and flag after a delay
        Invoke("ResetKick", 0.417f);
    }

    public void Header()
    {
        // Set header flag to true
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
}
