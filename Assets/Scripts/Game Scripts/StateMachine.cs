using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class StateMachine : MonoBehaviour
{
    public SpawnManager spawnManager;
    public BallInteraction ballInteraction;
    public PlayerMovement playerMovement;

    private GameState gameState;
    private int gameLength = 300;
    public float currentGameTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager.StartPos();
        currentGameTime = gameLength;
        SetGameState(GameState.Paused);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Playing)
        {
            currentGameTime -= Time.deltaTime;

            if (currentGameTime <= 0f)
            {
                SetGameState(GameState.GameOver);
            }
            
            if (ballInteraction.Goal())
            {
                GoalScored();
            }
        }
    }

    private void SetGameState(GameState newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameState.Playing:
                GamePlaying();
                break;
            case GameState.Paused:
                GamePaused();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }

    private void GamePlaying()
    {
        playerMovement.SetPaused(false);
    }

    private void GamePaused()
    {
        playerMovement.SetPaused(true);

        Invoke("DelayedGamePlaying", 3f);
    }

    private void DelayedGamePlaying()
    {
        SetGameState(GameState.Playing);
    }

    private void GoalScored()
    {
        spawnManager.StartPos();

        ballInteraction.ResetBall();

        SetGameState(GameState.Paused);
    }

    private void GameOver()
    {
        
        // Add any additional logic for game over state
    }
}
