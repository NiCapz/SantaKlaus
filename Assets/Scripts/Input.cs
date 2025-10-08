using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{

    public static Input Instance { get; private set; }
    private InputSystem_Actions playerInput;
    public Vector2 Move;
    public Vector2 Look;
    public bool sprintPressed = false;
    [SerializeField] private Player player;

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

        playerInput.Player.Look.performed += context => Look = context.ReadValue<Vector2>();
        playerInput.Player.Look.canceled += context => Look = Vector2.zero;

        playerInput.Player.Piss.performed += OnPissPressed;
        playerInput.Player.Piss.canceled += OnPissReleased;

        playerInput.Player.Interact.performed += OnInteractPressed;
        playerInput.Player.Interact.canceled += OnInteractReleased;

        playerInput.Player.Jump.performed += OnJumpPressed;
        playerInput.Player.Jump.canceled += OnJumpReleased;

    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        player.ToggleFuckControls();
    }
    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        player.UnfuckControls();
    }

    private void OnPissPressed(InputAction.CallbackContext context)
    {
        player.EnablePiss();
    }
    private void OnPissReleased(InputAction.CallbackContext context)
    {
        player.DisablePiss();
    }
    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        player.TryGrab();
    }
    private void OnInteractReleased(InputAction.CallbackContext context)
    {
        
    }

    //public bool GrabPressed() => playerInput.Player.Interact.WasPerformedThisFrame();
    public bool JumpPressed() => playerInput.Player.Jump.WasPerformedThisFrame();
    public bool SprintPressed() => playerInput.Player.Sprint.IsPressed();
    
}
