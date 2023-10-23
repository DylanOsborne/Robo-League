using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform car; // Reference to the car
    public Transform ball; // Reference to the ball

    // The empty GameObject that controls camera rotation (parented to the car)
    public Transform cameraPos;

    public float minFOV = 60.0f; // Minimum FOV
    public float maxFOV = 90.0f; // Maximum FOV
    public float zoomSpeed = 10.0f; // Camera zoom speed
    public float followSpeed = 5.0f; // Camera follow speed

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

    }

    void LateUpdate()
    {
        // Make the camera and cameraPos look at the ball
        cam.transform.LookAt(ball);
        cameraPos.transform.LookAt(ball);

        // Calculate the direction from the car to the ball
        Vector3 directionToBall = ball.position - car.position;

        // Calculate the desired FOV based on the distance to the ball
        float desiredFOV = Mathf.Clamp(directionToBall.magnitude, minFOV, maxFOV);

        // Smoothly adjust the camera's FOV
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFOV, Time.deltaTime * zoomSpeed);
    }
}
