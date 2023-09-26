using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Rigidbody rb;
    public Vector3 Offset;
    public float speed;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // Calculate the desired camera position
        Vector3 desiredPosition = player.position + player.transform.TransformVector(Offset);

        // Apply an additional offset in the direction opposite to the car's forward direction
        desiredPosition -= player.forward * 2f;

        // Use Lerp to smoothly interpolate between the current camera position and the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);

        // Make the camera look at the car
        transform.LookAt(player);
    }
}
