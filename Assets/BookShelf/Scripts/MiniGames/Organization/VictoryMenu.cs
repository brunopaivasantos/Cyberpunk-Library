using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI booksXpInfo;
    [SerializeField] TextMeshProUGUI timeXpInfo;
    [SerializeField] int startTimeXp = 180;
    [SerializeField] int booksXp = 25;
    [SerializeField] Transform levelStripe;
    [SerializeField] float stripeSpeed = 2f;
    [SerializeField] float xpSpeed = .04f;
    [SerializeField] TextMeshProUGUI currentLevel;
    [SerializeField] TextMeshProUGUI nextLevel;
    [SerializeField] TextMeshProUGUI levelInfo;
    [SerializeField] AudioSource levelStripeAudioSource;
    [SerializeField] AudioSource xpTextAudioSource;

    int timeXp;
    bool animating;
    Coroutine coroutine;

    float minPercentage;
    float maxPercentage;


    int totalXpEarned;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetMenu()
    {

        booksXpInfo.text = "Books Sorted __________ " + 0 + "XP";
        timeXpInfo.text = "Time Bonus __________ " + 0 + "XP";
        timeXp = startTimeXp - UI.GetTime();
        if (timeXp < 2)
            timeXp = 2;

        minPercentage = GetMinStripePercentage();
        levelStripe.localScale = new Vector3(minPercentage, levelStripe.localScale.y, levelStripe.localScale.z);
        int level = Statistics.currentLevel;
        currentLevel.text = Statistics.currentLevel.ToString().PadLeft(2, '0');
        nextLevel.text = (level + 1).ToString().PadLeft(2, '0');
        levelInfo.text = "LEVEL " + Statistics.currentLevel.ToString().PadLeft(2, '0');

        totalXpEarned = timeXp + booksXp;
        maxPercentage = GetMaxStripePercentage();

    }
    public void ShowMenu()
    {
        SetMenu();
        anim.SetTrigger("Open");
    }

    public void SetMenuCompleted()
    {
        GameManager.Instance.SetLevelFromXP();
        booksXpInfo.text = "Books Sorted __________ " + booksXp + "XP";
        timeXpInfo.text = "Time Bonus __________ " + timeXp + "XP";
        float minPercentage = GetMinStripePercentage();
        levelStripe.localScale = new Vector3(minPercentage, levelStripe.localScale.y, levelStripe.localScale.z);
        int level = Statistics.currentLevel;
        currentLevel.text = Statistics.currentLevel.ToString().PadLeft(2, '0');
        nextLevel.text = (level + 1).ToString().PadLeft(2, '0');
        levelInfo.text = "LEVEL " + Statistics.currentLevel.ToString().PadLeft(2, '0');

    }
    public void StartMenuAnimation()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(MenuAnimation());
    }

    public void StartNewGame()
    {
        UI.Instance.PlayClick();
        if (animating)
        {
            animating = false;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            SetMenuCompleted();
            return;
        }

        GameManager.Instance.LevelComplete(Statistics.currentLevel, 1, totalXpEarned);
        Shelf.Instance.SetGame();
    }

    public void HideMenu()
    {
        anim.SetTrigger("Close");
    }
    IEnumerator MenuAnimation()
    {
        animating = true;
        //Book XP
        float factor = 0;
        int xp = 0;
        while (factor <= 1)
        {
            factor += xpSpeed * Time.deltaTime;
            xp = (int)(factor * booksXp);
            if (!GameManager.soundMuted)
                xpTextAudioSource.Play();
            yield return null;//new WaitForSeconds(xpSpeed);

            booksXpInfo.text = "Books Sorted __________ " + xp + "XP";
        }

        Vector3 size = booksXpInfo.transform.localScale;
        booksXpInfo.transform.localScale = size * 1.1f;
        yield return new WaitForSeconds(.15f);
        booksXpInfo.transform.localScale = size;

        yield return new WaitForSeconds(.1f);


        //Time Bonus XP
        factor = 0;
        xp = 0;

        while (factor <= 1)
        {
            factor += xpSpeed * Time.deltaTime;

            xp = (int)(factor * timeXp);
            if (!GameManager.soundMuted)
                xpTextAudioSource.Play();
            yield return null;// new WaitForSeconds(xpSpeed * .2f);

            timeXpInfo.text = "Time Bonus __________ " + xp + "XP";
        }

        size = timeXpInfo.transform.localScale;
        timeXpInfo.transform.localScale = size * 1.1f;
        yield return new WaitForSeconds(.15f);
        timeXpInfo.transform.localScale = size;


        //LevelAnimation;

        float xpPercentage = minPercentage;

        Vector3 startStripeSize = new Vector3(minPercentage, levelStripe.localScale.y, levelStripe.localScale.z);
        Vector3 finalStripeSize = new Vector3(maxPercentage, levelStripe.localScale.y, levelStripe.localScale.z);
        factor = 0;
        while (factor < 1)
        {
            if (!GameManager.soundMuted)
                levelStripeAudioSource.Play();
            factor += stripeSpeed * Time.deltaTime;
            levelStripe.localScale = Vector3.Lerp(startStripeSize, finalStripeSize, factor);

            if (levelStripe.localScale.x >= 1)
            {
                factor = 0;
                float newXp = UpdateLevel();
                startStripeSize = new Vector3(0, levelStripe.localScale.y, levelStripe.localScale.z);
                finalStripeSize = new Vector3(newXp, levelStripe.localScale.y, levelStripe.localScale.z);


            }
            yield return null;
        }

        Statistics.Save();
        animating = false;
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

    float GetMaxStripePercentage()
    {
        (int, int) xpInterval = GameManager.Instance.GetXpInterval();
        int minXp = xpInterval.Item1;
        int maxXp = xpInterval.Item2;

        float total = maxXp - minXp;
        int newXp = Statistics.xp + booksXp + timeXp;
        GameManager.Instance.SetPlayerXP(newXp);
        Statistics.xp = newXp;

        float p = (newXp - minXp) / total;
        p = Mathf.Clamp(p, 0, 1);
        return p;
    }

    float UpdateLevel()
    {
        Statistics.currentLevel++;

        int level = Statistics.currentLevel;
        GameManager.Instance.SetPlayerLevel(level);
        currentLevel.text = Statistics.currentLevel.ToString().PadLeft(2, '0');
        nextLevel.text = (level + 1).ToString().PadLeft(2, '0');
        levelInfo.text = "LEVEL " + Statistics.currentLevel.ToString().PadLeft(2, '0');
        return GetMinStripePercentage();
    }

    float NextLevelStripePercentage()
    {
        (int, int) xpInterval = GameManager.Instance.GetXpInterval();
        int minXp = xpInterval.Item1;
        int maxXp = xpInterval.Item2;

        float total = maxXp - minXp;
        int newXp = Statistics.xp - minXp;
        Statistics.xp = newXp;

        float p = (newXp - minXp) / total;
        p = Mathf.Clamp(p, 0, 1);
        return p;
    }
}
