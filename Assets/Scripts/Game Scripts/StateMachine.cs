using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public SpawnManager spawnManager;
    public BallInteraction ballInteraction;

    private bool gameActive;
    private int gameLength = 300;
    public float currentGameTime;

    // Start is called before the first frame update
    void Start()
    {
        currentGameTime = gameLength;
        gameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            currentGameTime -= 1 * Time.deltaTime;

            if (ballInteraction.Goal())
            {
                GameReset();
            }
        }
    }

    private void GamePaused()
    {
        gameActive = false;
    }

    public void GameReset()
    {
        gameActive = false;
        spawnManager.StartPos();
    }    
}
