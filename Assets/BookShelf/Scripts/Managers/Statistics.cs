using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Statistics : Singleton<Statistics>
{
    [SerializeField] bool resetSavings;
    public static int currentLevel;
    public static int xp;

    public override void Awake()
    {
        
        if (resetSavings)
        {
            PlayerPrefs.DeleteAll();
        }
        Load();
    }
    private void Start()
    {
        
    }
    public static void Load()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        xp = PlayerPrefs.GetInt("xp", 0);
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("xp", xp);
    }

    public static bool IsSoundMuted()
    {
        return PlayerPrefs.GetInt("sound", 0) == 1;
    }

    public static void SaveSound(bool muted)
    {
        int save = muted ? 1 : 0;
        PlayerPrefs.SetInt("sound", save);
    }

}
