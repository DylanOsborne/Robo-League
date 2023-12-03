using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;  // Key to trigger a jump.
    public KeyCode punchKey = KeyCode.Mouse0;  // Key to trigger a punch.
    public KeyCode kickKey = KeyCode.Mouse1;  // Key to trigger a kick.
    public KeyCode headerKey = KeyCode.Mouse2;  // Key to trigger a header.
    public KeyCode walkKeyW = KeyCode.W;  // Key for forward movement.
    public KeyCode walkKeyA = KeyCode.A;  // Key for left movement.
    public KeyCode walkKeyS = KeyCode.S;  // Key for backward movement.
    public KeyCode walkKeyD = KeyCode.D;  // Key for right movement.
    public KeyCode sprintKey = KeyCode.LeftShift; // Key to activate sprinting
}
