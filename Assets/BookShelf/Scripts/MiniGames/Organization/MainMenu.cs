using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] PlayableDirector introScene;

    private void Start()
    {
    }

    public void StartGame()
    {
        if (!GameManager.soundMuted)
            this.GetComponent<AudioSource>().Play();
        startButton.SetActive(false);
        introScene.Play();
        introScene.stopped += Game;
    }
    private void OnDisable()
    {
        introScene.stopped -= Game;
    }
    public void Game(PlayableDirector playable)
    {
        GameManager.StartGame();
    }


}
