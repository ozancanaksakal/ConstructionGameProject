using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    public static InputManager Instance { get; private set; }
    public Vector2 MovementInput {  get; private set; }

    private void Awake() {
        Instance = this;
    }
    
    private void Update() {
        float yInput = Input.GetAxisRaw("Vertical");
        float xInput = Input.GetAxisRaw("Horizontal");

        MovementInput = new Vector2(xInput, yInput).normalized;
    }

    public static Vector2 GetMovementInput() => Instance.MovementInput;
}
