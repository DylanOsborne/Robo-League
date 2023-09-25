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
        CheckInput();
        ApplyMotor();
        ApplyBreak();
        ApplySteering(); 
        ApplyWheelRotation();
    }

    void CheckInput() 
    {
        gasInput = Input.GetAxis("Vertical") * -1;
        steeringInput = Input.GetAxis("Horizontal");
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