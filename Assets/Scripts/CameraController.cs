using UnityEngine;
using TMPro;

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

    private bool ballCam = true; // Whether or not ball cam is enabled

    private Camera cam;

    private string textValue = "Ball Cam On"; // The text used to display ball cam status
    public TMP_Text textElement; // Reference to the text element

    void Start()
    {
        cam = GetComponent<Camera>(); // Initialize the camera

        textElement.text = textValue; // Initialize the ball cam status text

        float width = (float)(Screen.width - (Screen.width * 0.9)); // Setting the derired width placement for the text
        float height = (float)(Screen.height - (Screen.height * 0.9)); // Setting the derired height placement for the text

        Vector3 newPos = new Vector3(width, height, 0); // Creating new vector for derired text placement

        textElement.transform.position = newPos; // Updating the text positioning
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ballCam = !ballCam; // Changing ball cam status on key press

        textElement.text = textValue; // Updating the ball cam status text
    }

    void LateUpdate()
    {

        if(ballCam)
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

            textValue = "Ball Cam On";
        }
        else
        {
            // Calculate the direction from the car to the ball
            Vector3 directionToBall = ball.position - car.position;

            // Calculate the desired FOV based on the distance to the ball
            float desiredFOV = Mathf.Clamp(directionToBall.magnitude, minFOV, maxFOV);

            // Smoothly adjust the camera's FOV
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFOV, Time.deltaTime * zoomSpeed);

            textValue = "Ball Cam Off";
        }
    }
}
