using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance;
    [SerializeField] TextMeshProUGUI objective;
    [SerializeField] Image clock;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI level;
    static float time = 0;
    Shelf manager;
    static bool startGame;
    float timeToCheck = .5f;
    void Awake()
    {
        Instance = this;
        manager = Shelf.Instance;
        startGame = false;
    }

    public void SetObjective(string objectiveText)
    {
        objective.text = objectiveText;

    }

    public void SetLevel()
    {
        level.text = "Level "+Statistics.currentLevel.ToString().PadLeft(2, '0');
    }
    void Update()
    {
        if (!startGame) return;
        time += Time.deltaTime;
        timeToCheck +=Time.deltaTime;
        //if(timeToCheck > .5f)
        //{
        //    timeToCheck = 0;
        //    Shelf.Instance.CheckVictory();
        //}
        UpdateClock();
    }

    public static void StartGame()
    {
        time = 0;
        startGame = true;
    }

    public static  void StopGame()
    {
        startGame = false;
    }

    private void UpdateClock()
    {
        timeText.text = FormatTime(time);
    }

    public static string FormatTime(float timer)
    {
        double time = timer;
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        //   int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}", minutes, seconds);//, milliseconds);
    }

    public static int GetTime()
    {
        return (int)time;
    }


}
