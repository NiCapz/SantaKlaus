using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PresentDespawner : MonoBehaviour
{

    [SerializeField] private Player player;

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider);
        player.IncrementPresentCounter();
    }

}
