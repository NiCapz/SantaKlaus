using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{

    private Rigidbody physicsBody;
    private bool taken = false;
    private GameObject attachPoint;
    [SerializeField] Vector3 offset = Vector3.zero;

    void Awake()
    {
        physicsBody = GetComponent<Rigidbody>();
        if (physicsBody == null)
        {
            physicsBody = GetComponentInChildren<Rigidbody>();
        }
    }

    void Update()
    {
        if (taken)
        {
            //transform.localPosition = offset;
        }
    }

    public void Take(Player player)
    {
        //transform.localPosition += Offset;
        transform.SetParent(player.attachPoint.transform);
        attachPoint = player.attachPoint;
        //transform.localPosition = Offset;
        transform.localPosition = Vector3.zero;
        if (physicsBody != null)
        {
            physicsBody.isKinematic = true;
            physicsBody.detectCollisions = false;
        }
        transform.localPosition = offset;
        taken = true;
    }

    public void Drop(Vector3 direction)
    {
        transform.SetParent(null);
        if (physicsBody != null)
        {
            physicsBody.isKinematic = false;
            physicsBody.detectCollisions = true;
        }
        taken = false;

        physicsBody.AddForce(direction, ForceMode.Impulse);
    }

}
