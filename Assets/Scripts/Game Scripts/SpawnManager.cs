using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Rigidbody ballRigidbody;

    [Header("SpawnPositions")]
    public Transform ballSpawnPos;
    public Transform playerSpawnPos1;
    public Transform playerSpawnPos2;
    public Transform playerSpawnPos3;
    public Transform playerSpawnPos4;
    public Transform playerSpawnPos5;
    public Transform playerSpawnPos6;

    public void StartPos()
    {
        // Set initial positions for player and ball
        playerRigidbody.position = playerSpawnPos1.position;
        playerRigidbody.rotation = playerSpawnPos1.rotation;

        ballRigidbody.position = ballSpawnPos.position;
    }
}
