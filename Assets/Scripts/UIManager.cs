using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaText;

    StaminaSystem staminaSystem;

    // Start is called before the first frame update
    void Start()
    {
        // Set the X position of the text
        Vector3 newTextWidth = staminaText.transform.position;
        newTextWidth.x = Screen.width * 0.95f;
        staminaText.transform.position = newTextWidth;

        // Set the y position of the text
        Vector3 newTextHeight = staminaText.transform.position;
        newTextHeight.y = Screen.height * 0.3f;
        staminaText.transform.position = newTextHeight;
    }

    // Update is called once per frame
    void Update()
    {
        staminaText.text = Mathf.RoundToInt(staminaSystem.currentStamina).ToString();
    }
}
