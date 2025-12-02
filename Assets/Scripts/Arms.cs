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
        Debug.Log("grab hit method");
    }

    public void Test()
    {
        Debug.Log("test");
    }

    public void BatHitTime()
    {
        player.CheckForBatHit();
    }


}
