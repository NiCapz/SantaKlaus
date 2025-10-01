using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{

    private CharacterController controller;

    [SerializeField] private Input input;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] private Transform cameraPivot;

    private float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

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
        Vector2 twoDMoveDir = Input.Instance.Move; // use your singleton input
        Vector3 moveDir = transform.right * twoDMoveDir.x + transform.forward * twoDMoveDir.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        controller.Move(moveDir * speed * Time.deltaTime);

        /*
        if (input.GrabPressed())
        {
            Debug.Log("Grab pressed");
        }
        if (input.JumpPressed())
        {
            Debug.Log("jump pressed");
        }*/

    }
}
