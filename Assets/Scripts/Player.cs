using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;

    [SerializeField] private Input input;

    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float walkSpeed = 10f;
    private float speed;

    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private Transform cameraPivot;

    private float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        speed = walkSpeed;
    }

    void Update()
    {

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

        controller.Move(moveDir * speed * Time.deltaTime);

        if (input.JumpPressed())
        {
            Debug.Log("jump pressed");
        }

    }
}
