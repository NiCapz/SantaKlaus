using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{

    private Rigidbody physicsBody;
    private bool taken = false;
    [SerializeField] Vector3 offset = Vector3.zero;
    private GameObject attachPoint;
    private Player playerObject;

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
        playerObject = player;
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

    public void Drop(float thrustPower)
    {
        transform.SetParent(null);
        if (physicsBody != null)
        {
            physicsBody.isKinematic = false;
            physicsBody.detectCollisions = true;
        }
        taken = false;

        physicsBody.AddForce(playerObject.transform.forward * thrustPower, ForceMode.Impulse);
    }

}
