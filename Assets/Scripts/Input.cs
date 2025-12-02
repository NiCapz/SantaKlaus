using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Countdown countdown;

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

        playerInput.Player.Interact.performed += OnInteractPressed;
        playerInput.Player.Interact.canceled += OnInteractReleased;

        playerInput.Player.Jump.performed += OnJumpPressed;

        playerInput.Player.Sprint.performed += OnSprintPressed;
        playerInput.Player.Sprint.canceled += OnSprintReleased;
        
        playerInput.Player.Bat.performed += OnBatPressed;

        playerInput.Player.Countdown.performed += StartCountDown;

        playerInput.Player.MiddleFinger.performed += MiddleFinger; 



    }

    private void OnBatPressed(InputAction.CallbackContext context)
    {
        player.ToggleBat();
        Debug.Log("toggling bat ig");
    }

    private void MiddleFinger(InputAction.CallbackContext context)
    {
        player.MiddleFingerPressed();
    }

    private void StartCountDown(InputAction.CallbackContext context)
    {
        countdown.InitiateCountdown();
    }

    private void OnSprintPressed(InputAction.CallbackContext context)
    {
        player.SetDesiredSpeed(Player.TargetSpeed.SprintSpeed);
    }
    private void OnSprintReleased(InputAction.CallbackContext context)
    {
        player.SetDesiredSpeed(Player.TargetSpeed.WalkSpeed);
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        player.Jump();
    }

    private void OnPissPressed(InputAction.CallbackContext context)
    {
        player.TogglePiss();
    }

    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        player.TryGrab();
    }
    private void OnInteractReleased(InputAction.CallbackContext context)
    {
        player.Release();
    }
    


    //public bool GrabPressed() => playerInput.Player.Interact.WasPerformedThisFrame();
    public bool JumpPressed() => playerInput.Player.Jump.WasPerformedThisFrame();
    public bool SprintPressed() => playerInput.Player.Sprint.IsPressed();
    
}
