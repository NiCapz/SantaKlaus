using UnityEngine;

public class Microwave : MonoBehaviour
{
    private bool open = false;
    bool containsBottle = false;
    private Transform doorBone;
    Vector3 closedAngles = new Vector3(0, -90, 90);
    Vector3 openAngles = new Vector3(0, 0, 90);


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
