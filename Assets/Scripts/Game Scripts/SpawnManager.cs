using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Rigidbody playerRigidbody;  // Reference to the player's Rigidbody component.
    public Rigidbody ballRigidbody;  // Reference to the Rigidbody of the ball.

    [Header("SpawnPositions")]
    public Transform ballSpawnPos;
    public Transform playerSpawnPos1;
    public Transform playerSpawnPos2;
    public Transform playerSpawnPos3;
    public Transform playerSpawnPos4;
    public Transform playerSpawnPos5;
    public Transform playerSpawnPos6;

    void Start()
    {
        playerRigidbody.position = playerSpawnPos1.position;
        playerRigidbody.rotation = playerSpawnPos1.rotation;

        ballRigidbody.position = ballSpawnPos.position;
    }
}
