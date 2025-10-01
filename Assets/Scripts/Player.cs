using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;

    [SerializeField] private Input input;
    [SerializeField] private float speed = 5.0f;
    

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 twoDMoveDir = input.Move;
        Vector3 moveDir = new Vector3(twoDMoveDir.x, 0, twoDMoveDir.y);
        controller.Move(moveDir * Time.deltaTime * speed);
    }
}
