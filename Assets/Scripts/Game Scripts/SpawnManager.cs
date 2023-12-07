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

    public int gameMode = 1;

    private Transform[] spawnPos;

    private void Start()
    {
        // Initialize the spawnPos array with specific elements
        if (gameMode == 1)
        {
            spawnPos = new Transform[] { playerSpawnPos1, playerSpawnPos4 };
        } 
        else if(gameMode == 2)
        {
            spawnPos = new Transform[] { playerSpawnPos2, playerSpawnPos3, playerSpawnPos5, playerSpawnPos6 };
        } 
        else if( gameMode == 3)
        {
            spawnPos = new Transform[] { playerSpawnPos1, playerSpawnPos2, playerSpawnPos3, playerSpawnPos4, playerSpawnPos5, playerSpawnPos6 };
        }
    }

    public void StartPos()
    {
        // Set initial positions for player and ball
        playerRigidbody.position = playerSpawnPos1.position;
        playerRigidbody.rotation = playerSpawnPos1.rotation;

        ballRigidbody.position = ballSpawnPos.position;
    }
}
