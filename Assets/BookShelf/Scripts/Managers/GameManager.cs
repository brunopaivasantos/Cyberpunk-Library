using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] List<int> xpToNextLevel = new List<int>();
    [SerializeField] List<GameMode> gameModes = new List<GameMode>();

    public static void StartGame()
    {
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
        if(levelIndex >= xpToNextLevel.Count)
        {
            return (0, 250);
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
}
