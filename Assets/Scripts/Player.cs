using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{
    // Player Singleton
    public static Player Instance { get; private set; }
    public enum TargetSpeed { SprintSpeed = 10, WalkSpeed = 5, CrouchSpeed = 2 }

    // Components
    [SerializeField] private Animator armsAnimator;
    [SerializeField] private Animator armsWithBatAnimator;
    [SerializeField] private Input input;
    [SerializeField] private Transform cameraPivot;
    private CharacterController controller;
    private ParticleSystem pissSystem;
    public GameObject attachPoint;
    private Stopwatch stopwatch;
    [SerializeField] private GameObject arms;
    [SerializeField] private GameObject armsWithBat;


    // Constants
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private float jumpHeight = .35f;
    [SerializeField] private float grabRange = 5f;
    [SerializeField] private float interactRange = 10f;
    [SerializeField] private float throwingPower = 1f;
    [SerializeField] private float maxThrowCharge = 1000f;



    // state - movement stats
    private Vector3 velocity;
    private float currentSpeed;
    private float desiredSpeed;
    private float xRotation = 0f;

    // state - inventory
    private Pickup heldItem;

    // state - binary values
    public bool pissing = true;
    private float cameraFlip = 0f;
    private bool fuckedControls = false;
    private int invertControls = 1;
    private bool isGrounded;
    private bool hasBat = true;
    private bool holdsBat = false;

    // game stats
    public static int presentCounter = 0;
    public static bool turkeyOnTree = false;
    public static bool microwaveExploded = false;

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
        stopwatch = new Stopwatch();
        desiredSpeed = (float)TargetSpeed.WalkSpeed;
        currentSpeed = (float)TargetSpeed.WalkSpeed;

        armsWithBat.SetActive(false);
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
        Vector2 twoDMoveDir = Input.Instance.Move;
        Vector3 moveDir = transform.right * twoDMoveDir.x + transform.forward * twoDMoveDir.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);
        moveDir *= invertControls;
        moveDir *= currentSpeed;
        if (!isGrounded) velocity.y += gravity * Time.deltaTime;
        moveDir += velocity;
        controller.Move(moveDir * Time.deltaTime);
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


    public void ToggleBat()
    {
        if (hasBat)
        {
            if (holdsBat)
            {
                arms.SetActive(true);
                armsWithBat.SetActive(false);
            }
            else
            {
                arms.SetActive(false);
                armsWithBat.SetActive(true);
            }
            holdsBat = !holdsBat;
        }
    }

    public void MiddleFingerPressed()
    {
        armsAnimator.SetTrigger("middlefinger");
        UnityEngine.Debug.Log("middleFinger?");
    }


    // interaction functions
    public void SetGrabbingFalse()
    {
        if (!holdsBat)
        {
            // called by the animator near the end of the grabbing animation, emits raycast for item and reenables ability to grab
            armsAnimator.SetBool("grabbing", false);
            if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out RaycastHit hit, grabRange))
            {
                heldItem = hit.collider.GetComponent<Pickup>();
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (heldItem) heldItem.Take();
                if (!heldItem && interactable) interactable.TryMicrowaveInteract();
            }
        }
    }
    public void TryGrab()
    {
        if (!holdsBat)
        {
            // Callback for the InputSystem, called if the interact button was pressd
            if (heldItem == null)
            {
                armsAnimator.SetBool("grabbing", true);
                return;
            }
            else if (heldItem.GetComponent<Breakable>()) stopwatch.Start();
            else if (heldItem.GetComponent<Interactable>())
            {
                Interactable interactable = heldItem.GetComponent<Interactable>();

                if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out RaycastHit hit, interactRange))
                {
                    interactable.TryInteract(hit);
                }
            }
        }
        else
        {
            armsWithBatAnimator.SetTrigger("hit");
        }
    }

    public void CheckForBatHit()
    {
        if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out RaycastHit hit, 3f))
        {
            Breakable breakable = hit.collider.gameObject.GetComponent<Breakable>();
            if (breakable != null) breakable.HitWithBat();
        }
    }

    public void Release()
    {
        if (heldItem)
        {
            float thrustPower = stopwatch.ElapsedMilliseconds / 100 * throwingPower;
            Math.Clamp(thrustPower, 0, maxThrowCharge);
            heldItem.Drop(cameraPivot.transform.forward * thrustPower);
            heldItem = null;
            stopwatch.Stop();
            stopwatch.Reset();
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
    public static void IncrementPresentCounter()
    {
        presentCounter++;
        Counter.UpdateCounter(presentCounter);
    }
}
