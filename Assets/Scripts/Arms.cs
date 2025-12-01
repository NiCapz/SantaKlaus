using UnityEngine;

public class Arms : MonoBehaviour
{

    Player player;

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    public void GrabOver()
    {
        player.SetGrabbingFalse();
    }


}
