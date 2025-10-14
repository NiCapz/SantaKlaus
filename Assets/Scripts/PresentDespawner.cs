using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PresentDespawner : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider);
        Player.IncrementPresentCounter();
    }
}
