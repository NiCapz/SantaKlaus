using UnityEngine;

public class PresentDespawner : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider);
        Player.IncrementPresentCounter();
    }
}
