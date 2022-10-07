using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using LionStudios.Suite.Analytics;

using LionStudios.Suite.Debugging;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] List<int> xpToNextLevel = new List<int>();
    [SerializeField] List<GameMode> gameModes = new List<GameMode>();
    public static bool soundMuted = false;

    [SerializeField] List<AudioClip> bgms = new List<AudioClip>();
    [SerializeField] int bgmIndex;
    AudioSource bgmAudioSource;
   
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        bgmAudioSource = this.GetComponent<AudioSource>();
        soundMuted = Statistics.IsSoundMuted();
        MuteBGM(soundMuted);

        PlayBGM();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LionDebugger.Hide();
    }
    void PlayBGM()
    {
        bgmAudioSource.clip = bgms[bgmIndex];
        bgmAudioSource.Play();
    }

    public void MuteBGM(bool isMuted)
    {
        bgmAudioSource.mute = isMuted;
    }

    public static void StartGame()
    {
        LionAnalytics.GameStart();
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>how much xp was necessary to get in the currentLevel and the xp necessary to the next level</returns>
    public (int, int) GetXpInterval()
    {

        int level = Statistics.currentLevel;
        int levelIndex = level - 1;
        if (levelIndex >= xpToNextLevel.Count)
        {
            int lastIndex = xpToNextLevel.Count - 1;
            xpToNextLevel.Add(xpToNextLevel[lastIndex] + 250);
            return (xpToNextLevel[lastIndex], xpToNextLevel[lastIndex + 1]);
        }
        if (levelIndex == 0)
            return (0, xpToNextLevel[levelIndex]);
        else
            return (xpToNextLevel[levelIndex - 1], xpToNextLevel[levelIndex]);
    }


    public GameMode GetGameMode()
    {
        int index = Statistics.currentLevel - 1;
        if (index >= gameModes.Count)
        {
            index = Random.Range(15, gameModes.Count);
        }
        return gameModes[index];
    }

    public void SetLevelFromXP()
    {
        (int, int) interval = GetXpInterval();

        int maxXp = interval.Item2;

        if (Statistics.xp >= maxXp)
        {
            Statistics.currentLevel++;
            SetLevelFromXP();
        }
        else return;
    }


    public void LevelStart(int level, int attemptNum)
    {
        LionAnalytics.LevelStart(level, attemptNum);
    }

    public void LevelComplete(int level, int attemptNum, int ? score = null)
    {
        LionAnalytics.LevelComplete(level, attemptNum, score);
    }

    public void SetPlayerLevel(int playerLevel)
    {
        LionAnalytics.SetPlayerLevel(playerLevel);
    }
  
    public void SetPlayerXP(int playerXp)
    {
        LionAnalytics.SetPlayerXP(playerXp);
    }

}
