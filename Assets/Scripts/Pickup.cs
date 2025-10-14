using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{

    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] private Rigidbody physicsBody;
    private GameObject attachPoint;
    private Transform broken;
    public List<Transform> pieces = new List<Transform>();

    void Awake()
    {
        if (physicsBody == null)
            physicsBody = GetComponentInChildren<Rigidbody>();

        physicsBody.isKinematic = false;
        physicsBody.detectCollisions = true;

        if (broken = transform.Find("broken"))
        {
            pieces = broken.GetComponentsInChildren<Transform>()
                           .Where(t => t != broken)
                           .ToList();

            foreach (Transform pieceTransform in pieces)
            {
                if (pieceTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    pieceTransform.gameObject.SetActive(false);
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }
            }
        }
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

    public void Collide(float intensity)
    {
        Debug.Log($"{gameObject.name} collided with wall with an intensity of {intensity}");
        if (intensity > 5)
        {
            gameObject.SetActive(false);
            foreach (Transform pieceTransform in pieces)
            {
                pieceTransform.SetParent(null);
                Debug.Log("bleh");
                pieceTransform.gameObject.SetActive(true);
                Rigidbody rb = pieceTransform.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }
        }
    }

}
