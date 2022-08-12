using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    private static float time;
    private static bool running;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!running) return;

        time += Time.deltaTime;
    }


    public static void StartTimer()
    {
        running = true;
    }
    public static void Restart()
    {
        time = 0;
        running = true;
    }

    public static void  Stop()
    {
        running = false;
    }

    public static float GetTime()
    {
        return time;
    }

}
