using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform levelStripe;
    [SerializeField] TextMeshProUGUI currentLevel;
    [SerializeField] TextMeshProUGUI nextLevel;
    [SerializeField] TextMeshProUGUI levelInfo;
    [SerializeField] GameObject muteImage;
    [SerializeField] GameObject unmuteImage;

    bool soundMuted;
    Animator anim;
    bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        soundMuted = GameManager.soundMuted;
        muteImage.SetActive(!soundMuted);
        unmuteImage.SetActive(soundMuted);
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevel()
    {
        float minPercentage = GetMinStripePercentage();
        levelStripe.localScale = new Vector3(minPercentage, levelStripe.localScale.y, levelStripe.localScale.z);
        int level = Statistics.currentLevel;
        currentLevel.text = Statistics.currentLevel.ToString().PadLeft(2, '0');
        nextLevel.text = (level + 1).ToString().PadLeft(2, '0');
        levelInfo.text = "LEVEL " + Statistics.currentLevel.ToString().PadLeft(2, '0');
    }

    float GetMinStripePercentage()
    {
        (int, int) xpInterval = GameManager.Instance.GetXpInterval();
        int minXp = xpInterval.Item1;
        int maxXp = xpInterval.Item2;

        float total = maxXp - minXp;

        float p = (Statistics.xp - minXp) / total;

        p = Mathf.Clamp(p, 0, 1);
        return p;
    }

    public void ToggleSound()
    {
        soundMuted = !soundMuted;
        muteImage.SetActive(!soundMuted);
        unmuteImage.SetActive(soundMuted);
        GameManager.soundMuted = soundMuted;
        GameManager.Instance.MuteBGM(soundMuted);
        Statistics.SaveSound(soundMuted);
    }

    public void SetPause()
    {
        UI.Instance.PlayClick();
        if (isPaused)
        {
            Continue();
            return;
        }
        Time.timeScale = 0;
        SetLevel();
        anim.SetTrigger("Open");
        isPaused = true;
    }

    public void Continue()
    {
        UI.Instance.PlayClick();
        isPaused = false;
        Time.timeScale = 1;
        anim.SetTrigger("Close");
    }

    public void Home()
    {
        UI.Instance.PlayClick();
        Time.timeScale = 1;
        GameManager.Instance.MainMenu();
    }
}
