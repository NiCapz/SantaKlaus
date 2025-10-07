using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;
    [SerializeField] private Animator animator;

    [SerializeField] private Input input;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float walkSpeed = 10f;
    private float speed;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private Transform cameraPivot;

    [SerializeField] private GameObject attachPoint;
    private Transform heldItem;

    private float xRotation = 0f;

    public bool grabbing = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        speed = walkSpeed;
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

    void Update()
    {

        Debug.Log(grabbing);

        if (Input.Instance.GrabPressed())
        {
            grabbing = true;
            animator.SetBool("grabbing", true);
        }

        Vector2 look = Input.Instance.Look;

        float mouseX = look.x * lookSensitivity * Time.deltaTime;
        float mouseY = look.y * lookSensitivity * Time.deltaTime;

        // Pitch (camera up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Yaw (player body left/right)
        transform.Rotate(Vector3.up * mouseX);

        // ---- Movement ----
        if (Input.Instance.SprintPressed())
        {
            Debug.Log(Input.Instance.SprintPressed());
            //Mathf.Lerp(speed, sprintSpeed, 0.5f);
            speed = sprintSpeed;
        }
        else
        {
            //Mathf.Lerp(speed, walkSpeed, 0.5f);
            speed = walkSpeed;
        }

        Vector2 twoDMoveDir = Input.Instance.Move; // use your singleton input
        Vector3 moveDir = transform.right * twoDMoveDir.x + transform.forward * twoDMoveDir.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

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
