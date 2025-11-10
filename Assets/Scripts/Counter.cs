using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public static TextMeshProUGUI gui;

    void Start()
    {
        gui = GameObject.Find("Counter").GetComponent<TextMeshProUGUI>();
    }   

    public static void UpdateCounter(int count)
    {
        gui.SetText($"Presents destroyed: {count} / 11");
    }
}
