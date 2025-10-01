using UnityEngine;

public class Npc : MonoBehaviour
{

    [SerializeField] Player player;

    void Awake()
    {
        //player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
