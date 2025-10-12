using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{

    private Rigidbody physicsBody;
    private bool taken = false;
    public BoxCollider boxCollider;

    void Awake()
    {
        physicsBody = GetComponent<Rigidbody>();
        if (physicsBody == null)
        {
            physicsBody = GetComponentInChildren<Rigidbody>();
        }
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (taken)
        {
            transform.position = GetComponentInParent<Transform>().position;
        }
    }

    public void Take(Player player)
    {
        transform.SetParent(player.attachPoint.transform);
        transform.localPosition = Vector3.zero;
        if (physicsBody != null)
        {
            physicsBody.isKinematic = true;
            physicsBody.detectCollisions = false;
            
        }
        taken = true;
        if (boxCollider)
        {
            //boxCollider.enabled = false;
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        if (physicsBody != null)
        {
            //physicsBody.isKinematic = true;
            
        }
        if (boxCollider)
        {
            boxCollider.enabled = true;
        }
    }


}
