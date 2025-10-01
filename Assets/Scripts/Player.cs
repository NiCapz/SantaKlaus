using UnityEngine;

public class Player : MonoBehaviour
{
    private Input input;

    void Awake()
    {
        input = new Input();

        Vector2 moveDir = input.Move;


    }
}
