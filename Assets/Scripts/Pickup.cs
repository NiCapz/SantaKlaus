using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{

    private Rigidbody physicsBody;
    private bool taken = false;
    [SerializeField] Vector3 Offset = Vector3.zero;
    private GameObject attachPoint;

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
        Debug.Log($"Local Position: {transform.localPosition}");
        Debug.Log($"Global Position: {transform.position}");
        if (taken)
        {
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
        transform.localPosition = Offset;
        taken = true;
    }

    public void Drop()
    {
        transform.SetParent(null);
        if (physicsBody != null)
        {
            physicsBody.isKinematic = false;
            physicsBody.detectCollisions = true;
        }
    }


}
