using UnityEngine;

public class Arms : MonoBehaviour
{

    Player player;

    void Awake()
    {
        player = Player.Instance;
    }

    public void GrabOver()
    {
        player.SetGrabbingFalse();
    }


}
