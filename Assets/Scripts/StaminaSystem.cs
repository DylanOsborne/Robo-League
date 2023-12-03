using UnityEngine;
using TMPro;

public class StaminaSystem : MonoBehaviour
{
    public Animator animator;

    [Header("Stamina Settings")]
    private readonly float maxStamina = 100f;
    private readonly float sprintStaminaConsumptionRate = 35f;
    private readonly float staminaRechargeRate = 20f;

    private float currentStamina;

    public float moveSpeed = 60f;

    [SerializeField] private TextMeshProUGUI staminaText;

    private void Start()
    {
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();

        // Set the X position of the text
        Vector3 newTextWidth = staminaText.transform.position;
        newTextWidth.x = Screen.width * 0.95f;
        staminaText.transform.position = newTextWidth;

        // Set the y position of the text
        Vector3 newTextHeight = staminaText.transform.position;
        newTextHeight.y = Screen.height * 0.3f;
        staminaText.transform.position = newTextHeight;
    }

    private void Update()
    {
        staminaText.text = Mathf.RoundToInt(currentStamina).ToString();
    }

    public void Sprint()
    {
        if (currentStamina > 0)
        {
            // Consume stamina while sprinting
            currentStamina -= sprintStaminaConsumptionRate * Time.deltaTime;

            moveSpeed = 110f;
            animator.SetFloat("walkSpeed", 1.5f);
        }
        else
        {
            moveSpeed = 60f;
            animator.SetFloat("walkSpeed", 1f);
        }
    }

    public void RechargeStamina()
    {
        // Gradually recharge stamina when not sprinting
        currentStamina += staminaRechargeRate * Time.deltaTime;

        // Clamp the stamina value to the maximum
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Regular walking speed
        moveSpeed = 60f;
        animator.SetFloat("walkSpeed", 1f);
    }
}
