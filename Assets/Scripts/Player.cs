using UnityEngine;

public class Player : MonoBehaviour
{
    public enum TargetSpeed { SprintSpeed = 10, WalkSpeed = 5, CrouchSpeed = 2 }

    public static Player Instance { get; private set; }

    [SerializeField] private Animator animator;
    [SerializeField] private Input input;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] public GameObject attachPoint;

    private CharacterController controller;
    private ParticleSystem pissSystem;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float lookSensitivity = 50f;

    private float currentSpeed;
    private float desiredSpeed;

    private Pickup heldItem;

    private float xRotation = 0f;

    public bool grabbing = false;
    public bool pissing = false;

    private float cameraFlip = 0f;
    private int invertControls = 1;
    private bool fuckedControls = false;

    public void SetDesiredSpeed(TargetSpeed newDesiredSpeed)
    {
        desiredSpeed = (float) newDesiredSpeed;
    }

    private void LerpSpeedToDesired()
    {
        if (currentSpeed != desiredSpeed) currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, 0.5f);
    }

    void Update()
    {
        Debug.Log(currentSpeed);
        LerpSpeedToDesired();

        Vector2 look;
        look = Input.Instance.Look;
        look *= invertControls;

        float mouseX = look.x * lookSensitivity * Time.deltaTime;
        float mouseY = look.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, cameraFlip);

        transform.Rotate(Vector3.up * mouseX);

        Vector2 twoDMoveDir = Input.Instance.Move;
        Vector3 moveDir = transform.right * twoDMoveDir.x + transform.forward * twoDMoveDir.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);
        moveDir *= invertControls;

        if (!controller.isGrounded)
        {
            moveDir.y += gravity;
        }

        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        if (input.JumpPressed())
        {
            Debug.Log("jump pressed");
        }

    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        controller = GetComponent<CharacterController>();
        pissSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        desiredSpeed = (float)TargetSpeed.WalkSpeed;
        currentSpeed = (float)TargetSpeed.WalkSpeed;
        pissSystem.Pause();
    }

    public void EnablePiss()
    {
        pissSystem.Play();
        pissing = true;
    }

    public void DisablePiss()
    {
        pissSystem.Stop();
        pissing = false;
    }

    // called by the animator near the end of the grabbing animation
    // emits raycast for item and reenables ability to grab
    public void SetGrabbingFalse()
    {
        grabbing = false;
        animator.SetBool("grabbing", false);

        if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out RaycastHit hit, 1.5f))
        {
            heldItem = hit.collider.GetComponent<Pickup>();
            if (heldItem != null)
            {
                heldItem.Grab(this);
            }

        }
    }
    // Callback for the InputSystem, called if the interact button was pressd
    public void TryGrab()
    {
        if (heldItem == null)
        {
            grabbing = true;
            animator.SetBool("grabbing", true);
        }
        else
        {
            heldItem.Drop();
        }
    }

    public void ToggleFuckControls()
    {
        if (!fuckedControls)
        {
            cameraFlip = 180f;
            invertControls = -1;
            fuckedControls = true;
            gravity = 9.81f;
        }
        else
        {
            cameraFlip = 0f;
            invertControls = 1;
            fuckedControls = false;
            gravity = -9.81f;
        }
    }

    public void UnfuckControls()
    {
    }
}
