using UnityEngine;

public class Pickup : MonoBehaviour
{

    private Rigidbody physicsBody;

    void Awake()
    {
        physicsBody = GetComponent<Rigidbody>();
        if (physicsBody = null)
        {
            physicsBody = GetComponentInChildren<Rigidbody>();
        }
    }

    public void Grab(Player player)
    {
        transform.SetParent(player.attachPoint.transform);
        transform.localPosition = Vector3.zero;
        if (physicsBody != null)
        {
            physicsBody.isKinematic = false;
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        if (physicsBody != null)
        {
            physicsBody.isKinematic = true;
        }
    }


}
