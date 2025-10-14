using NUnit.Framework;
using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{


    void OnCollisionEnter(Collision collision)
    {
        float collisionIntensity = collision.relativeVelocity.magnitude;
        Pickup pickUp = collision.gameObject.GetComponent<Pickup>() ?? null;
        if (pickUp) pickUp.Collide(collisionIntensity);
    }

}
