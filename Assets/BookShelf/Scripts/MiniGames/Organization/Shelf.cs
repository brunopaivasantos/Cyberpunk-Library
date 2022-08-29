using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shelf : MonoBehaviour
{
    public static Shelf Instance;

    [SerializeField] Stand topShelf;
    [SerializeField] Stand middleShelf;
    [SerializeField] Stand bottomShelf;
    [SerializeField] List<Sprite> coloredBooks = new List<Sprite>();
    [SerializeField] UI ui;

    [SerializeField] VictoryMenu victory;

    [SerializeField] List<AudioClip> bookAudioClips = new List<AudioClip>();

    [SerializeField] AudioSource victoryAudio;
    [SerializeField] AudioSource introAudio;
    List<Sprite> gameColors = new List<Sprite>();

    List<Slot> slots = new List<Slot>();
    int slotLastIndex;

    List<Book> books = new List<Book>();
    int colorIndex = 0;
    List<int> ids = new List<int>();
    GameMode gameMode;
    public static bool organizing;
    public static bool gameOver;

    private void Awake()
    {
        Instance = this;

    }

    void SetUI()
    {
        switch (gameMode.gameMode)
        {
            case Enums.GameMode.color:
                ui.SetObjective("Sort books by Color");
                break;

            case Enums.GameMode.letter:
                ui.SetObjective("Sort books alphabetically");
                break;

            case Enums.GameMode.number:
                ui.SetObjective("Sort books by number");
                break;


            case Enums.GameMode.odds_evens:
                ui.SetObjective("Separate odd and even books");
                break;
        }

        ui.SetLevel();
    }

    void ResetGame()
    {
        if (gameMode == null) return;
        if (gameMode.standQuantity > 1)
        {
            topShelf.DestroyBooks();

        }
        middleShelf.DestroyBooks();



        if (gameMode.standQuantity > 2)
        {
            bottomShelf.DestroyBooks();

        }

        slots.Clear();
    }
    public void SetGame(bool fromTitleScreen = false)
    {
        ResetGame();
        gameOver = false;
        gameMode = GameManager.Instance.GetGameMode();


        SetUI();
        gameColors.Clear();
        int index = Random.Range(0, gameMode.colorsQuantity);


        for (int i = 0; i < gameMode.colorsQuantity; i++)
        {
            gameColors.Add(coloredBooks[i]);
        }
        colorIndex = 0;
        SetIdCollection();

        middleShelf.gameObject.SetActive(false);
        topShelf.gameObject.SetActive(false);
        bottomShelf.gameObject.SetActive(false);

        if (gameMode.standQuantity > 1)
        {
            topShelf.gameObject.SetActive(true);
            topShelf.SetBooks(fromTitleScreen);
        }


        middleShelf.gameObject.SetActive(true);
        middleShelf.SetBooks(fromTitleScreen);



        if (gameMode.standQuantity > 2)
        {
            bottomShelf.gameObject.SetActive(true);
            bottomShelf.SetBooks(fromTitleScreen);

        }

        SetSlots(fromTitleScreen);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetGame(true);
        // SetSlots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeLine(Slot slot, Book book, bool definitive)
    {

        int newIndex = slots.IndexOf(slot);
        bool toRight = slotLastIndex > newIndex;
        Debug.Log("Last Index =" + slotLastIndex + "    new Index = " + newIndex);
        slot.SetNewBook(toRight, book, definitive);
        slotLastIndex = newIndex;
    }

    void SetSlots(bool fromTitleScreen = false)
    {
        Slot topLastSlot = null;
        Slot firstMiddleSlot = middleShelf.GetSlots()[0];
        Slot lastMiddleSlot = middleShelf.GetSlots()[9];
        Slot bottomFirstSlot = null;

        if (gameMode.standQuantity > 1)
        {
            topLastSlot = topShelf.GetSlots()[9];
            topLastSlot.SetNextSlot(firstMiddleSlot);
            firstMiddleSlot.SetPreviousSlot(topLastSlot);

        }
        if (gameMode.standQuantity > 2)
        {
            bottomFirstSlot = bottomShelf.GetSlots()[0];
            lastMiddleSlot.SetNextSlot(bottomFirstSlot);
            bottomFirstSlot.SetPreviousSlot(lastMiddleSlot);
        }

        if (!GameManager.soundMuted)
            introAudio.Play();
        UI.StartGame();
        gameOver = false;
        if (!fromTitleScreen)
            victory.HideMenu();

    }
    public void SetLastIndex(Slot slot)
    {
        slotLastIndex = slots.IndexOf(slot);
    }
    public Sprite GetColor(Book book)
    {
        colorIndex++;

        if (colorIndex >= gameColors.Count) colorIndex = 0;

        book.SetTextColor(colorIndex);
        return gameColors[colorIndex];
    }
    public int CreateId()
    {
        if (gameMode.gameMode == Enums.GameMode.color) return 0;
        int index = Random.Range(0, ids.Count);
        int n = ids[index];
        ids.RemoveAt(index);
        return n;
    }

    public Enums.GameMode GetGameMode()
    {
        return gameMode.gameMode;
    }

    void SetIdCollection()
    {
        if (gameMode.gameMode == Enums.GameMode.color) return;


        if (gameMode.idQuantity == (gameMode.maxId) - gameMode.minId)
        {
            for (int i = gameMode.minId; i < gameMode.maxId; i++)
            {
                ids.Add(i);
            }
            return;
        }
        HashSet<int> idCollection = new HashSet<int>();

        while (idCollection.Count < gameMode.idQuantity)
        {
            idCollection.Add(Random.Range(gameMode.minId, gameMode.maxId + 1));
        }

        foreach (int i in idCollection)
        {
            ids.Add(i);
        }
    }

    public void CheckVictory()
    {
        switch (gameMode.gameMode)
        {

            case Enums.GameMode.color:
                CheckColors();
                break;

            case Enums.GameMode.letter:
                CheckId();
                break;

            case Enums.GameMode.odds_evens:
                CheckOddsEvens();
                break;

            case Enums.GameMode.number:
                CheckId();
                break;



        }
    }

    void CheckColors()
    {
        List<Sprite> spritesInLine = new List<Sprite>();
        Book book = slots[0].GetCurrentBook();
        if (book == null) return;
        Sprite currentSprite = book.GetColor();
        spritesInLine.Add(currentSprite);
        Sprite lastSprite = book.GetColor();
        foreach (Slot s in slots)
        {
            currentSprite = s.GetCurrentBook().GetColor();
            if (lastSprite == currentSprite)
            {
                lastSprite = currentSprite;
            }
            if (lastSprite != currentSprite)
            {
                if (spritesInLine.Contains(currentSprite))
                {
                    return;
                }
                else
                {
                    spritesInLine.Add(currentSprite);
                    lastSprite = currentSprite;
                }
            }
        }

        EndGame();
    }

    void CheckId()
    {
        int id = -1;
        foreach (Slot s in slots)
        {
            int nextId = s.GetCurrentBook().GetID();
            if (id > nextId) return;
            id = nextId;
        }

        EndGame();
    }

    void CheckOddsEvens()
    {


        int id = slots[0].GetCurrentBook().GetID();
        bool isEven = id % 2 == 0;
        int value = isEven ? 0 : 1;
        int nextValue = 0;
        bool changed = false;
        foreach (Slot s in slots)
        {
            id = s.GetCurrentBook().GetID();
            nextValue = id % 2 == 0 ? 0 : 1;

            if (value == nextValue)
            {
                value = nextValue;
            }
            else
            {
                if (changed) return;
                else
                {
                    changed = true;
                    value = nextValue;
                }
            }

        }
        EndGame();
    }
    void EndGame()
    {
        if (!GameManager.soundMuted)
            victoryAudio.Play();
        UI.StopGame();
        victory.ShowMenu();
    }
    //{
    //    if (gameMode == Enums.GameMode.color)
    //    {
    //        bool endedFirstGroup = false;
    //        bool endedSecondGroup = false;
    //        Color firstColor = middleSlots[0].GetComponent<Slot>().GetBook().GetColor();
    //        Color secondColor = middleSlots[0].GetComponent<Slot>().GetBook().GetColor();
    //        Color thirdColor = middleSlots[0].GetComponent<Slot>().GetBook().GetColor();
    //        foreach (Transform s in middleSlots)
    //        {
    //            Color color = s.GetComponent<Slot>().GetBook().GetColor();


    //            if (color != firstColor && !endedFirstGroup)
    //            {
    //                endedFirstGroup = true;
    //                secondColor = color;
    //            }
    //            else if (color == firstColor && endedFirstGroup)
    //            {
    //                return;
    //            }

    //            if (color != secondColor && !endedSecondGroup)
    //            {
    //                endedSecondGroup = true;
    //                thirdColor = color;
    //            }
    //            else if (color == secondColor && endedSecondGroup)
    //            {
    //                return;
    //            }

    //        }
    //    }
    //    else
    //    {
    //        int id = -1;
    //        foreach (Transform s in middleSlots)
    //        {
    //            int nextId = s.GetComponent<Slot>().GetBook().GetID();
    //            if (id > nextId) return;
    //            id = nextId;
    //        }
    //    }


    //    ui.SetResult("Nice");

    //}

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public Sprite GetRandomBookSprite(Book book)
    {

        int index = Random.Range(0, gameColors.Count);
        book.SetTextColor(index);
        return gameColors[index];
    }

    public void AddSlot(Slot slot)
    {
        slots.Add(slot);
    }

    public void PlayRandomSound()
    {
        if (GameManager.soundMuted) return;
        int index = Random.Range(0, bookAudioClips.Count);
        this.GetComponent<AudioSource>().clip = bookAudioClips[index];
        this.GetComponent<AudioSource>().Play();
    }
}
