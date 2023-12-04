using UnityEngine;

public class StateMachine : MonoBehaviour
{
    bool gameActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (gameActive)
        {

        }
    }

    private void GameStarts()
    {
        gameActive = true;
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
