using UnityEngine;

public class Microwave : MonoBehaviour
{
    private bool open = false;
    bool containsBottle = false;
    private Transform doorBone;
    Vector3 closedAngles = new Vector3(0, -90, 90);
    Vector3 openAngles = new Vector3(0, 0, 90);

    public void StartMicrowave()
    {
        ToggleOpen();

        transform.localEulerAngles = new Vector3(0, 0, 90);
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        TimerManager.StartTimer(2f, () =>
        {
            var audio = GetComponentInParent<AudioSource>();
            audio.gameObject.SetActive(true);
            audio.Play();
            Breakable br = GetComponentInParent<Breakable>();
            br.Explode();
        });
        Player.microwaveExploded = true;
    }

    void Awake()
    {
        doorBone = GameObject.Find("DoorBone").transform;
        ToggleOpen();
    }

    public void InsertBottle()
    {
        containsBottle = true;
    }

    public bool GetContainsBottle()
    {
        return containsBottle;
    }

    public bool GetOpen()
    {
        return open;
    }

    public void ToggleOpen()
    {
        open = !open;
        doorBone.localEulerAngles = open ? openAngles : closedAngles;
    }

    public void Interact()
    {
        if (!open) ToggleOpen();

    }
}
