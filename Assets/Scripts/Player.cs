using UnityEngine;

public class Player : MonoBehaviour
{
    // Player Singleton
    public static Player Instance { get; private set; }
    public enum TargetSpeed { SprintSpeed = 10, WalkSpeed = 5, CrouchSpeed = 2 }

    // Components
    [SerializeField] private Animator animator;
    [SerializeField] private Input input;
    [SerializeField] private Transform cameraPivot;
    private CharacterController controller;
    private ParticleSystem pissSystem;
    public GameObject attachPoint;

    // Constants
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private float jumpHeight = .35f;

    // state - stats
    private Vector3 velocity;
    private float currentSpeed;
    private float desiredSpeed;
    private float xRotation = 0f;

    // state - inventory
    private Pickup heldItem;

    // state - binary values
    public bool pissing = false;
    private float cameraFlip = 0f;
    private bool fuckedControls = false;
    private int invertControls = 1;
    private bool isGrounded;

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

    void Update()
    {
        LerpSpeedToDesired();
        Look();
        Move();
    }

    void Look()
    {
        Vector2 look;
        look = Input.Instance.Look;
        look *= invertControls;

        float mouseX = look.x * lookSensitivity * Time.deltaTime;
        float mouseY = look.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, cameraFlip);

        transform.Rotate(Vector3.up * mouseX);
    }
    public void Jump()
    {
        if (isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        //if (isGrounded) velocity.y = -2f;

        Vector2 twoDMoveDir = Input.Instance.Move;
        Vector3 moveDir = transform.right * twoDMoveDir.x + transform.forward * twoDMoveDir.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);
        moveDir *= invertControls;


        if (!isGrounded) velocity.y += gravity * Time.deltaTime;

        moveDir += velocity;
        controller.Move(moveDir * currentSpeed * Time.deltaTime);
        //controller.Move(velocity * Time.deltaTime);
    }

    //movement functions
    public void SetDesiredSpeed(TargetSpeed newDesiredSpeed)
    {
        desiredSpeed = (float)newDesiredSpeed;
    }
    private void LerpSpeedToDesired()
    {
        if (currentSpeed != desiredSpeed) currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, 0.5f);
    }

    // interaction functions
    public void SetGrabbingFalse()
    {
        // called by the animator near the end of the grabbing animation
        // emits raycast for item and reenables ability to grab
        Debug.Log("grabbing false");
        animator.SetBool("grabbing", false);

        if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out RaycastHit hit, 1.5f))
        {
            heldItem = hit.collider.GetComponent<Pickup>();
            if (heldItem) heldItem.Take(this);
        }
    }
    public void TryGrab()
    {
        // Callback for the InputSystem, called if the interact button was pressd
        if (heldItem == null) animator.SetBool("grabbing", true);
        else
        {
            heldItem.Drop();
            heldItem = null;
        }
    }

    // immersion methods
    public void TogglePiss()
    {
        if (!pissing)
        {
            pissSystem.Play();
            pissing = true;
        }
        else
        {
            pissSystem.Stop();
            pissing = false;
        }
    }
    public void DisablePiss()
    {
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
}
