using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] private Rigidbody physicsBody;
    private GameObject attachPoint;
    public List<Transform> pieces = new List<Transform>();


    void Awake()
    {
        if (physicsBody == null)
            physicsBody = GetComponentInChildren<Rigidbody>();

        physicsBody.isKinematic = false;
        physicsBody.detectCollisions = true;
    }

    public void Take(Player player)
    {
        transform.SetParent(player.attachPoint.transform);
        attachPoint = player.attachPoint;
        transform.localPosition = Vector3.zero;
        if (physicsBody != null)
        {
            physicsBody.isKinematic = true;
            physicsBody.detectCollisions = false;
        }
        transform.localPosition = offset;
    }

    public void Drop(Vector3 direction)
    {
        transform.SetParent(null);
        if (physicsBody != null)
        {
            physicsBody.isKinematic = false;
            physicsBody.detectCollisions = true;
        }
        physicsBody.AddForce(direction, ForceMode.Impulse);
    }
}
