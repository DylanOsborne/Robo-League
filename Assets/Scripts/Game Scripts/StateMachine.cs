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
    public Animator animator;
    public StaminaSystem staminaSystem;

    private GameState gameState;
    private int gameLength = 300;
    public float currentGameTime;

    void Start()
    {
        // Set initial positions and game time
        spawnManager.StartPos();
        currentGameTime = gameLength;
        SetGameState(GameState.Paused);
    }

    void Update()
    {
        if (gameState == GameState.Playing)
        {
            // Update game time and check for goal
            currentGameTime -= Time.deltaTime;

            if (currentGameTime <= 0f)
            {
                // If game time is up, set the game state to GameOver
                SetGameState(GameState.GameOver);
            }
            
            if (ballInteraction.Goal())
            {
                // If a goal is scored, trigger the GoalScored function
                GoalScored();
            }
        }
    }

    private void SetGameState(GameState newState)
    {
        gameState = newState;

        // Execute specific actions based on the new game state
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
        // Allow player movement when in playing state
        playerMovement.SetPaused(false);
    }

    private void GamePaused()
    {
        // Pause player movement and delay resuming after 3 seconds
        playerMovement.SetPaused(true);
        Invoke("DelayedGamePlaying", 3f);
    }

    private void DelayedGamePlaying()
    {
        // Resume playing state after the delay
        SetGameState(GameState.Playing);
    }

    private void GoalScored()
    {
        // Reset values, pause the game, and set the state to Paused
        ResetValues();
        SetGameState(GameState.Paused);
    }

    private void ResetValues()
    {
        // Reset player and ball positions, animations, and stamina
        spawnManager.StartPos();
        ballInteraction.ResetBall();

        animator.SetBool("walking", false);
        animator.SetBool("punch", false);
        animator.SetBool("kick", false);
        animator.SetBool("header", false);
        animator.SetBool("grounded", true);

        staminaSystem.currentStamina = 100;
    }

    private void GameOver()
    {
        
    }
}
