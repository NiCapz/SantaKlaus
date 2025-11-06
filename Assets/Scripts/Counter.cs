using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public static TextMeshProUGUI gui;

    void Awake()
    {
        gui = GetComponent<TextMeshProUGUI>();
    }

    public static void UpdateCounter(int count)
    {
        gui.SetText($"Presents destroyed: {count} / 11");
    }
}
