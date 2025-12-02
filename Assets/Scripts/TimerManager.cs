using System.Collections;
using UnityEngine;
using System;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public static void StartTimer(float duration, Action callback)
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("TimerManager");
            Instance = go.AddComponent<TimerManager>();
            //DontDestroyOnLoad(go);
        }
        Instance.StartCoroutine(Instance.TimerCoroutine(duration, callback));
    }

    private IEnumerator TimerCoroutine(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }

    

}
