using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI rGoalText;
    [SerializeField] private TextMeshProUGUI bGoalText;

    public StaminaSystem staminaSystem;
    public StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "05:00";
        staminaText.text = "100";
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(stateMachine.currentGameTime);
        timeText.text = time.ToString("mm':'ss");
        staminaText.text = Mathf.RoundToInt(staminaSystem.currentStamina).ToString();
    }
}
