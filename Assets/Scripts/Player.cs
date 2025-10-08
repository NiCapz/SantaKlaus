using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private Input input;
    private ParticleSystem pissSystem;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float walkSpeed = 5f;
    private float speed;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private Transform cameraPivot;

    [SerializeField] private GameObject attachPoint;
    private Transform heldItem;

    private float xRotation = 0f;

    public bool grabbing = false;
    public bool pissing = false;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        pissSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        speed = walkSpeed;
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


    public void SetGrabbingFalse()
    {
        animator.SetBool("grabbing", false);
        grabbing = false;

        if (Physics.Raycast(cameraPivot.position, cameraPivot.TransformDirection(Vector3.forward), out RaycastHit hit, 1.5f))
        {
            heldItem = hit.transform;
            heldItem.SetParent(attachPoint.transform);
            heldItem.localPosition = Vector3.zero;
        }
    }

    public void TryGrab()
    {
        if (heldItem == null)
        {
            grabbing = true;
            animator.SetBool("grabbing", true);
        }
        else
        {
            heldItem.transform.parent = null;
        }
    }

    void Update()
    {

        Vector2 look = Input.Instance.Look;

        float mouseX = look.x * lookSensitivity * Time.deltaTime;
        float mouseY = look.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        if (Input.Instance.SprintPressed())
        {
            Mathf.Lerp(speed, sprintSpeed, 0.5f);
            speed = sprintSpeed;
        }
        else
        {
            Mathf.Lerp(speed, walkSpeed, 0.5f);
            speed = walkSpeed;
        }

        Vector2 twoDMoveDir = Input.Instance.Move; // use your singleton input
        Vector3 moveDir = transform.right * twoDMoveDir.x + transform.forward * twoDMoveDir.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);
        //moveDir = -moveDir;

        if (!controller.isGrounded)
        {
            moveDir.y += gravity;
        }

        controller.Move(moveDir * speed * Time.deltaTime);

        if (input.JumpPressed())
        {
            Debug.Log("jump pressed");
        }

    }
}
