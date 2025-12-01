using UnityEditor.PackageManager;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Transform interactionPartner;
    Transform attachPoint;
    [SerializeField] bool turkey;
    [SerializeField] bool wineBottle;
    [SerializeField] bool microWave;
    private Rigidbody rb;

    void Awake()
    {
        if (interactionPartner) attachPoint = interactionPartner.Find("AttachPoint");
        rb = GetComponent<Rigidbody>();
    }



    public bool TryInteract(RaycastHit hit)
    {
        if (hit.transform == interactionPartner)
        {
            transform.SetParent(interactionPartner);

            rb.isKinematic = true;
            transform.localEulerAngles = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.position = attachPoint.position;
            try
            {
                Destroy(gameObject.GetComponent<Pickup>());
            }
            catch
            {
                Debug.Log("no pickup found");
            }
            
            if (turkey)
            {
                Player.turkeyOnTree = true;
                transform.localEulerAngles = new Vector3(0, 90, 180);
            }
            if (wineBottle)
            {
                var microWave = interactionPartner.GetComponent<Microwave>();
                microWave.ToggleOpen();
                Destroy(microWave);

                transform.localEulerAngles = new Vector3(0, 0, 90);
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
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
            Destroy(this);
        }

        return true;
    }

    public void TryMicrowaveInteract()
    {
        if (microWave) gameObject.GetComponent<Microwave>().ToggleOpen();
    }

}
