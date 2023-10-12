using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;
    public WheelColliders colliders;
    public WheelMesh wheelMeshes;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float groundCheckRadius;

    private float gasInput;
    private float brakeInput;
    private float steeringInput;

    public float motorPower;
    public float brakePower;
    private float speed;

    public float jumpForce;
    private bool secondJump = false;
    public bool grounded = false; // Indicates whether the car is grounded or in the air

    public string direction; // Tracks the car's direction

    private readonly float bankAngle = 360f;
    public readonly float bankSpeed = 25f;
    private readonly float airTurnSpeed = 10f; // Torque applied for air control

    public AnimationCurve steeringCurve;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {

        grounded = false; // Reset grounded status
        speed = rb.velocity.magnitude;

        CheckInput(); // Check user input
        ApplyMotor(); // Apply motor torque to wheels
        ApplyBreak(); // Apply braking force to wheels
        ApplySteering(); // Apply steering angle to front wheels
        ApplyWheelRotation(); // Update wheel rotation
        GroundCheck(); // Check if the car is grounded

        if (!grounded) // Car is in the air
        {
            AirControl(); // Control the car's orientation in the air

            AirRoll(); // Perform an air roll

            if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                rb.angularVelocity = Vector3.zero; // Stop angular velocity when no keys are pressed
            }
        }

        if (grounded) // Car is on the ground
        {
            secondJump = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump(); // Perform a jump when the Space key is pressed
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && secondJump)
        {
            Jump(); // Perform a jump in the air if the second jump is available
            secondJump = false;
        }
    }

    void Jump()
    {
        // Calculate the jump direction as the negative of the car's current up direction
        Vector3 jumpDirection = transform.up;

        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
    }

    void AirControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Apply torque to rotate the car forward (pitch)
            Vector3 rotationTorque = -bankAngle * airTurnSpeed * transform.right;
            rb.AddTorque(rotationTorque);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Apply torque to rotate the car backward (pitch)
            Vector3 rotationTorque = bankAngle * airTurnSpeed * transform.right;
            rb.AddTorque(rotationTorque);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Apply torque to rotate the car left (yaw)
            Vector3 rotationTorque = -bankAngle * airTurnSpeed * transform.up;
            rb.AddTorque(rotationTorque);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Apply torque to rotate the car right (yaw)
            Vector3 rotationTorque = bankAngle * airTurnSpeed * transform.up;
            rb.AddTorque(rotationTorque);
        }
        else
        {
            rb.angularVelocity = Vector3.zero; // When nothing pressed, stop angular velocity
        }
    }

    void AirRoll()
    {
        if (Input.GetKey(KeyCode.Q)) // Perform an air roll to the left
        {
            Vector3 rotationTorque = -bankAngle * bankSpeed * transform.forward;

            rb.AddTorque(rotationTorque);
        }
        else if (Input.GetKey(KeyCode.E)) // Perform an air roll to the right
        {
            Vector3 rotationTorque = bankAngle * bankSpeed * transform.forward;

            rb.AddTorque(rotationTorque);
        }
        else
        {
            rb.angularVelocity = Vector3.zero; // When nothing pressed, stop angular velocity
        }
    }

    void GroundCheck()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer); // Check if the car is grounded
    }

    void CheckInput()
    {
        gasInput = 0;
        steeringInput = 0;


        if (Input.GetKey(KeyCode.W))
        {
            if (direction == "R" && speed > 1)
            {
                brakeInput = 1;
            }
            else
            {
                brakeInput = 0;
                direction = "F";
                gasInput = -1; // Apply gas to move forward
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (direction == "F" && speed > 1)
            {
                brakeInput = 1;
            }
            else
            {
                brakeInput = 0;
                direction = "R";
                gasInput = 1; // Apply gas to move backward
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            steeringInput = -1; // Steer left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            steeringInput = 1; // Steer right
        }
    }

    void ApplyMotor()
    {
        colliders.BRWheel.motorTorque = motorPower * gasInput;
        colliders.BLWheel.motorTorque = motorPower * gasInput;
    }

    void ApplyBreak()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

        colliders.BRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        colliders.BLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
    }

    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelRotation()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.BRWheel, wheelMeshes.BRWheel);
        UpdateWheel(colliders.BLWheel, wheelMeshes.BLWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        coll.GetWorldPose(out Vector3 position, out Quaternion quat);
        wheelMesh.transform.SetPositionAndRotation(position, quat);
    }
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider BRWheel;
    public WheelCollider BLWheel;
}

[System.Serializable]
public class WheelMesh
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer BRWheel;
    public MeshRenderer BLWheel;
}