using UnityEngine;

public class Arms : MonoBehaviour
{

    [SerializeField] Player player;

    public void GrabOver()
    {
        player.SetGrabbingFalse();
    }


}
