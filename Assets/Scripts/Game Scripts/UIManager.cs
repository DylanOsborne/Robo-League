using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public RectTransform canvasRect;

    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI rGoalText;
    [SerializeField] private TextMeshProUGUI bGoalText;

    public StaminaSystem staminaSystem;
    public StateMachine stateMachine;

    void Start()
    {
        // Initialize default values
        timeText.text = "05:00";
        staminaText.text = "100";
    }

    void Update()
    {
        // Update time, stamina, and goal text
        TimeSpan time = TimeSpan.FromSeconds(stateMachine.currentGameTime);
        timeText.text = time.ToString("mm':'ss");
        staminaText.text = Mathf.RoundToInt(staminaSystem.currentStamina).ToString();

        rGoalText.text = stateMachine.redGoals.ToString();
        bGoalText.text = stateMachine.blueGoals.ToString();
    }
}
