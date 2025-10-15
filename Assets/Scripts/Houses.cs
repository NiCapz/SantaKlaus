using Unity.VisualScripting;
using UnityEngine;

public class Houses : MonoBehaviour
{
    void Start()
    {
        int childCount = transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).AddComponent<Wall>();
        }
    }
}
