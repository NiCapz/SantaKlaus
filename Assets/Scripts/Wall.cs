using UnityEngine;

public class Wall : MonoBehaviour
{
    
    void OnCollisionEnter(Collision collision)
    {
        float collisionIntensity = collision.relativeVelocity.magnitude;
        Breakable breakable = collision.gameObject.GetComponent<Breakable>() ?? null;
        if (breakable) breakable.Collide(collisionIntensity);
    }
    
}
