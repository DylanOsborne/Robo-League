using UnityEngine;

public class StateMachine : MonoBehaviour
{
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
        }
    }

    private void GamePaused()
    {
        gameActive = false;
    }

    private void GameReset()
    {
        gameActive = false;
    }

    private void GoalScored()
    {
        gameActive = false;
    }

    
}
