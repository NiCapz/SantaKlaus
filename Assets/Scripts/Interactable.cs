using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Transform interactionPartner;
    Transform attachPoint;
    [SerializeField] bool turkeyOnChristmasTree;

    void Awake()
    {
        attachPoint = interactionPartner.Find("AttachPoint");
    }

    public bool TryInteract(RaycastHit hit)
    {

        if (hit.transform == interactionPartner)
        {
            if (turkeyOnChristmasTree)
            {
                transform.localEulerAngles = Vector3.zero;
                transform.localPosition = Vector3.zero;
                transform.position = attachPoint.position;
                Player.turkeyOnTree = true;
            }
        }

        return true;
    }

}
