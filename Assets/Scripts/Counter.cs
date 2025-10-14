using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    private TextMeshProUGUI gui;

    void Awake()
    {
        gui = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCounter(int count)
    {
        gui.SetText($"Presents destroyed: {count}");
    }
}
