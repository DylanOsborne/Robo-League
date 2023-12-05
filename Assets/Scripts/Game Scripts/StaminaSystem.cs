using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public Animator animator;

    [Header("Stamina Settings")]
    private const float maxStamina = 100f;
    private const float sprintStaminaConsumptionRate = 35f;
    private const float staminaRechargeRate = 20f;

    public float currentStamina;

    public float moveSpeed = 60f;

    private void Start()
    {
        // Initialize stamina and animator
        currentStamina = maxStamina;
        animator = GetComponent<Animator>();
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
            // Reset speed if stamina is depleted
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
