using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private float timeout;
    [SerializeField] private TextMeshProUGUI countdown;
    [SerializeField] private TextMeshProUGUI finalText;
    private static float timeRemaining;
    [SerializeField] private MeshCollider[] colliders;


    void Start()
    {
        timeRemaining = 30;
    }

    public void InitiateCountdown()
    {
        TimerManager.StartTimer(1f, () => CountDownOneSecond());
    }

    void CountDownOneSecond()
    {
        timeRemaining -= 1;
        countdown.SetText($"Time remaining: {timeRemaining}");
        if (timeRemaining > 0)
        {
            TimerManager.StartTimer(1f, () => CountDownOneSecond());
        }
        else
        {
            //MeshCollider[] colliders = GetComponents<MeshCollider>();
            foreach (MeshCollider collider in colliders)
            {
                Destroy(collider.GameObject());
                //collider.enabled = false;
            }

            countdown.SetText("");
            Counter.gui.SetText("");

            int presentCounter = Player.presentCounter;
            switch (presentCounter)
            {
                case < 3:
                    finalText.SetText($"You have destroyed {Player.presentCounter} out of 11 presents.\nChristmas is still fun.\n Do better.");
                    break;
                case < 7:
                    finalText.SetText($"You have destroyed {Player.presentCounter} out of 11 presents.\n One Child was crying, but it could be worse");
                    break;
                case < 11:
                    finalText.SetText($"You have destroyed {Player.presentCounter} out of 11 presents.\n The children are miserable. Well done.\n But next time, finish the job.");
                    break;
                case < 12:
                    finalText.SetText($"You have destroyed {Player.presentCounter} out of 11 presents.\nCongrats, you got all the presents,\nChristmas is ruined and you can be proud");
                    break;
            }
            
            //finalText.SetText($"You have destroyed {Player.presentCounter} out of 11 presents.\nYou shall return next year, to ruin christmas again.");
            
        }

    }
}
