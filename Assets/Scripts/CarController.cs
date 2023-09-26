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

    public float gasInput;
    public float brakeInput;
    public float steeringInput;

    public float motorPower;
    public float brakePower;
    private float speed;

    public float jumpForce;

    public bool canJump = false;
    public bool secondJump = false;

    public AnimationCurve steeringCurve;

    // Start is called before the first frame update 
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame 
    void Update()
    {
        bool isGrounded = GroundCheck();

        speed = rb.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplyBreak();
        ApplySteering(); 
        ApplyWheelRotation();

        if (isGrounded)
        {
            canJump = true;
            secondJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
            canJump = false;
        } 
        else if(Input.GetKeyDown(KeyCode.Space) && secondJump)
        {
            Jump();
            secondJump = false;
        }
    }

    void Jump()
    {
        // Apply an upward force to the car's Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool GroundCheck()
    {
        // Cast a sphere from the ground check position to check for ground contact.
        // Adjust the radius and layer mask according to your car's size and the ground's layer.
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void CheckInput() 
    {
        gasInput = Input.GetAxis("Vertical") * -1;
        steeringInput = Input.GetAxis("Horizontal");
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
        colliders.BRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.BLWheel.brakeTorque = brakeInput * brakePower * 0.7f;
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