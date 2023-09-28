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

    public string direction;

    private float bankAngle = 360f;
    private float bankSpeed = 2f;

    public AnimationCurve steeringCurve;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        speed = rb.velocity.magnitude;

        CheckInput();
        ApplyMotor();
        ApplyBreak();
        ApplySteering(); 
        ApplyWheelRotation();

        bool isGrounded = GroundCheck();

        if (!isGrounded)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            if (Input.GetKey(KeyCode.Q))
            {
                AirRoll(1);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                AirRoll(2);
            }
        }

        if (isGrounded)
        {
            secondJump = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && secondJump)
        {
            Jump();
            secondJump = false;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void AirRoll(int side)
    {
        float bankAmount = 0f;

        rb.constraints = RigidbodyConstraints.None;

        if (side == 1)
        {
            bankAmount = -bankAngle;
        }
        else if (side == 2)
        {
            bankAmount = bankAngle;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, bankAmount);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * bankSpeed);
    }

    bool GroundCheck()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void CheckInput() 
    {
        gasInput = 0;
        steeringInput = 0;
        

        if (Input.GetKey(KeyCode.W))
        {
            if(direction == "R" && speed > 1)
            {
                brakeInput = 1;
            } 
            else
            {
                brakeInput = 0;
                direction = "F";
                gasInput = -1;
            }
        }
        else if(Input.GetKey(KeyCode.S))
        {
            if(direction == "F" && speed > 1)
            {
                brakeInput = 1;
            }
            else
            {
                brakeInput = 0;
                direction = "R";
                gasInput = 1;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            steeringInput = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            steeringInput = 1;
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