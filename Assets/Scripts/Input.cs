using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{

    public static Input Instance { get; private set; }
    private InputSystem_Actions playerInput;
    public Vector2 Move;
    public Vector2 Look;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInput = new InputSystem_Actions();
        playerInput.Player.Enable();

        Cursor.lockState = CursorLockMode.Locked;

        playerInput.Player.Move.performed += context => Move = context.ReadValue<Vector2>();
        playerInput.Player.Move.canceled += context => Move = Vector2.zero;
    }

    void Update()
    {
        Look = playerInput.Player.Look.ReadValue<Vector2>();
    }
    
    public bool GrabPressed() => playerInput.Player.Interact.WasPerformedThisFrame();
    public bool JumpPressed() => playerInput.Player.Jump.WasPerformedThisFrame();
}
