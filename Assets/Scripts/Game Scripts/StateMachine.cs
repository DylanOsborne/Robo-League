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
        spawnManager.StartPos();
        currentGameTime = gameLength;
        SetGameState(GameState.Paused);
    }

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
        ResetValues();

        SetGameState(GameState.Paused);
    }

    private void ResetValues()
    {
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
