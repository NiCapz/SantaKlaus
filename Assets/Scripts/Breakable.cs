using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    private Transform broken;
    public List<Transform> pieces = new List<Transform>();
    private float requiredBreakageVelocity = 5f;

    void Awake()
    {
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

    public void Collide(float intensity)
    {
        //Debug.Log($"{gameObject.name} collided with wall with an intensity of {intensity}");
        if (intensity > requiredBreakageVelocity)
        {
            gameObject.SetActive(false);
            Player.IncrementPresentCounter();
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
