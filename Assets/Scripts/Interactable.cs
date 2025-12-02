using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Transform interactionPartner;
    Transform attachPoint;
    [SerializeField] bool turkey;
    [SerializeField] bool wineBottle;
    [SerializeField] bool microWave;
    private Rigidbody rb;

    private AudioSource[] audioSources;
    

    void Awake()
    {
        if (interactionPartner) attachPoint = interactionPartner.Find("AttachPoint");
        rb = GetComponent<Rigidbody>();
        if (wineBottle || turkey) audioSources = interactionPartner.gameObject.GetComponentsInChildren<AudioSource>();
    }


    public bool TryInteract(RaycastHit hit)
    {
        Debug.Log(gameObject.name + "tried to interact");
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
                audioSources[0].Play();
                Player.turkeyOnTree = true;
                transform.localEulerAngles = new Vector3(0, 90, 180);
                transform.localPosition += new Vector3(0, 0.0033f, 0);
            }
            if (wineBottle)
            {
                var microWave = interactionPartner.GetComponent<Microwave>();

                microWave.ToggleOpen();

                transform.localEulerAngles = new Vector3(0, 0, 90);
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                //AudioSource[] audioSources = interactionPartner.gameObject.GetComponentsInChildren<AudioSource>();
                //audioSources[0].gameObject.SetActive(true);
                // audioSources[1].gameObject.SetActive(true);
                audioSources[0].Play();

                TimerManager.StartTimer(5f, () =>
                {
                    audioSources[0].Stop();
                    audioSources[1].Play();
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
