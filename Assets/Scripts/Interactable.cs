using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Transform interactionPartner;
    Transform attachPoint;
    [SerializeField] bool turkeyOnChristmasTree;
    [SerializeField] bool wineBottle;
    private Rigidbody rb;

    void Awake()
    {
        attachPoint = interactionPartner.Find("AttachPoint");
        rb = GetComponent<Rigidbody>();
    }

    public bool TryInteract(RaycastHit hit)
    {
        if (hit.transform == interactionPartner)
        {
            rb.isKinematic = true;
            transform.localEulerAngles = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.position = attachPoint.position;

            if (turkeyOnChristmasTree)
            {
                Player.turkeyOnTree = true;
            }
            if (wineBottle)
            {
                transform.localEulerAngles = new Vector3(0, 0, 90);
                TimerManager.StartTimer(2f, () =>
                {
                    Transform audio = interactionPartner.Find("AudioSource");
                    audio.gameObject.SetActive(true);
                    audio.GetComponent<AudioSource>().Play();
                    Breakable br = interactionPartner.GetComponent<Breakable>();
                    br.Explode();
                });
                Player.microwaveExploded = true;
            }
        }

        return true;
    }

}
