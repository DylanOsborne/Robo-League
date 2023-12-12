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

        // Initialize text placements using percentages
        /*SetTextPosition(staminaText.rectTransform, 0.9f, 0.2f);
        SetTextPosition(timeText.rectTransform, 0.5f, 0.8f);
        SetTextPosition(rGoalText.rectTransform, 0.4f, 0.8f);
        SetTextPosition(bGoalText.rectTransform, 0.6f, 0.8f);*/
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

    void SetTextPosition(RectTransform textTransform, float xPercent, float yPercent)
    {
        // Get the canvas rect size
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Calculate position based on percentage
        float xPos = canvasWidth * xPercent;
        float yPos = canvasHeight * yPercent;

        // Set the anchored position
        textTransform.anchoredPosition = new Vector2(xPos, yPos);
    }
}
