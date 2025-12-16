using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{

    public enum Counters {present, interaction, smash}

    public static TextMeshProUGUI presentCounter;
    public static TextMeshProUGUI smashCounter;
    public static TextMeshProUGUI interactionCounter;

    static int presentTotal = 6;
    static int interactionsTotal = 2;
    static int smashTotal = 7;

    void Start()
    {
        presentCounter = GameObject.Find("PresentCounter").GetComponent<TextMeshProUGUI>();
        smashCounter = GameObject.Find("SmashCounter").GetComponent<TextMeshProUGUI>();
        interactionCounter = GameObject.Find("InteractionCounter").GetComponent<TextMeshProUGUI>();
    }



    public static void UpdateCounter(int count, Counters counter)
    {
        
        switch (counter)
        {
            case Counters.present:
                if (presentCounter && count <= presentTotal) presentCounter.SetText($"Presents destroyed: {count} / {presentTotal}");
                break;
            case Counters.interaction:
                if (interactionCounter && count <= interactionsTotal) interactionCounter.SetText($"Interactions found: {count} / {interactionsTotal}");
                break;
            case Counters.smash:
                if (smashCounter && count <= smashTotal) smashCounter.SetText($"Objects smashed: {count} / {smashTotal}");
                break;

        }
        //else Debug.Log("No gui object present");
    }
}
