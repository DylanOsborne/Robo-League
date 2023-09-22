using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;
    public WheelColliders colliders;
    public WheelMesh wheelMeshes;

    public float gasInput;
    public float brakeInput;
    public float steeringInput;

    public float motorPower;
    public float brakePower;
    private float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;

    // Start is called before the first frame update 
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame 
    void Update()
    {
        speed = rb.velocity.magnitude;
        checkInput();
        ApplyMotor();
        ApplyBreak();
        ApplySteering(); 
        ApplyWheelRotation();
    }

    void checkInput() 
    {
        gasInput = Input.GetAxis("Vertical") *-1;
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, rb.velocity-transform.forward);

        if(slipAngle < 120f) {
            if(gasInput < 0) {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
        } else {
                brakeInput = 0;
        }
    }

    void ApplyMotor()
    {
        colliders.FRWheel.motorTorque = motorPower * gasInput;
        colliders.FLWheel.motorTorque = motorPower * gasInput;
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
        steeringAngle += Vector3.SignedAngle(transform.forward, rb.velocity + transform.forward, Vector3.up);
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
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
        Quaternion quat;
        Vector3 position;

        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
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