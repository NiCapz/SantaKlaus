using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    private Transform broken;
    public List<Transform> pieces = new List<Transform>();
    private float requiredBreakageVelocity = 5f;
    private float explosionforce = 1f;
    private float explosionRadius = 1f;
    [SerializeField] private int hp;
    [SerializeField] bool present;

    void Awake()
    {
        if (broken = transform.GetChild(0))
        {
            pieces = broken.GetComponentsInChildren<Transform>().Where(t => t != broken).ToList();

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

    public void HitWithBat()
    {
        hp -= 1;
        Debug.Log($"hit {gameObject.name}, {hp} remaining");
        if (hp <= 0)
        {
            Player.IncrementSmashCounter();
            Break();
        }
    }

    public void Explode()
    {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        //gameObject.SetActive(false);
        /*
        foreach(Transform child in GetComponentInChildren<Transform>())
        {
            child.SetParent(null);
            child.gameObject.SetActive(true);
            
        } */

        GetComponent<BoxCollider>().enabled = false;
        foreach (MeshRenderer ren in transform.Find("MicroWaveVisual").GetComponentsInChildren<MeshRenderer>())
        {
            ren.enabled = false;
        }
        foreach (SkinnedMeshRenderer ren in transform.Find("MicroWaveVisual").GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            ren.enabled = false;
        }

        Rigidbody rb;
        foreach (Transform piece in pieces)
        {
            if (rb = piece.GetComponent<Rigidbody>())
            {
                piece.GetComponent<MeshRenderer>().enabled = true;
                piece.transform.SetParent(null);
                piece.gameObject.SetActive(true);
                rb.isKinematic = false;
                rb.detectCollisions = true;
                rb.AddExplosionForce(explosionforce, transform.position, explosionRadius);
            }
        }
    }

    private void Break()
    {
        gameObject.SetActive(false);
        if (present) Player.IncrementPresentCounter();
        //else Player.IncrementSmashCounter();
        var audioSource = gameObject.GetComponentInChildren<AudioSource>();
        if (audioSource)
        {
            audioSource.transform.SetParent(null);
            audioSource.gameObject.SetActive(true);
            audioSource.enabled = true;
            audioSource.Play();
        }

        foreach (Transform pieceTransform in pieces)
        {
            pieceTransform.SetParent(null);
            pieceTransform.gameObject.SetActive(true);
            Rigidbody rb = pieceTransform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.detectCollisions = true;

            TimerManager.StartTimer(2f, () => TimerRunout(pieces));
        }
    }

    public void Collide(float intensity)
    {
        //Debug.Log($"{gameObject.name} collided with wall with an intensity of {intensity}");
        if (intensity > requiredBreakageVelocity)
        {
            Break();
        }
    }

    static void TimerRunout(List<Transform> piecesToDestroy)
    {
        foreach (Transform piece in piecesToDestroy)
        {
            //piece.GetComponent<Rigidbody>().isKinematic = true;
            //piece.GetComponent<Rigidbody>().detectCollisions = false;
            var renderer = piece.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }
    }

}
