using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private TextMeshProUGUI RGoalText;
    [SerializeField] private TextMeshProUGUI BGoalText;

    public StaminaSystem staminaSystem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        staminaText.text = Mathf.RoundToInt(staminaSystem.currentStamina).ToString();
    }
}
