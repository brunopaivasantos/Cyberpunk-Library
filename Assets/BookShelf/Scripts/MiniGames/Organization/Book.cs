using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Book : MonoBehaviour
{
    private Vector3 position;
    private bool onHands;
    [SerializeField] private float speed;
    [SerializeField] List<Color> textColors = new List<Color>();
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI romanInfo;
    [SerializeField] ParticleSystem particles;
    Color dustColor;
    ParticleSystem dust;
    int id;
    Vector3 size;
    Shelf shelf;
    Slot currentSlot;
    Slot newSlot;
    bool returning;

    private void Awake()
    {

        // GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Count)];
    }
    // Start is called before the first frame update
    public void Start()
    {
        shelf = Shelf.Instance;
        size = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onHands) return;
        if (Vector3.Distance(transform.position, position) < .05f)
        {
            // onHands = false;
            // transform.localScale = size;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);

    }

    public void SetHands(bool isOnHands)
    {

        onHands = isOnHands;
        if (isOnHands)
        {
            PlayerInput.occupied = true;
            SendDust();
            this.GetComponent<SpriteRenderer>().sortingLayerName = "Hands";
            info.transform.parent.GetComponent<Canvas>().sortingLayerName = "Hands";
            transform.localScale = size * 1.3f;
            this.GetComponent<SpriteRenderer>().sortingOrder = 10;
            info.transform.parent.GetComponent<Canvas>().sortingOrder = 11;
        }
        if (!onHands)
        {
            this.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            info.transform.parent.GetComponent<Canvas>().sortingLayerName = "Default";
            transform.localScale = size;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
            info.transform.parent.GetComponent<Canvas>().sortingOrder = 2;

        }

    }

    public void SendDust()
    {
        Shelf.Instance.PlayRandomSound();
        dust = Instantiate(particles, this.transform.position, Quaternion.identity);
        ParticleSystem.MainModule main = dust.main;
        main.startColor = dustColor;
        //dust.startColor = dustColor;

        Destroy(dust.gameObject, 1.3f);
    }
    public void Return()
    {
        PlayerInput.occupied = true;
        currentSlot.SetNewBook(true, this, true, currentSlot);
        StartCoroutine(GoingToSlot(currentSlot));
    }
    public void GoToSlot(Slot slot)
    {
        StartCoroutine(GoingToSlot(slot));
    }
    public void SetPosition(Vector3 pos)
    {
        position = pos;
    }

    public void SetSlot(Slot slot)
    {
        currentSlot = slot;
    }

    //public void SetNewSlot(Slot newSlot)
    //{
    //    if (newSlot == null) return;
    //    SetPosition(newSlot.transform.position);
    //    Shelf.Instance.ChangeLine(currentSlot.transform, newSlot.transform, this);
    //    currentSlot = newSlot;
    //    // newSlot.ChangeBook(this, currentSlot);

    //    //this.GetComponent<Collider2D>().enabled = true;
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shelf")
        {
        }
        else if (collision.tag == "Slot")
        {
            newSlot = collision.GetComponent<Slot>();


        }

    }

    IEnumerator GoingToSlot(Slot slot)
    {
        float factor = 0;

        Vector3 start = this.transform.position;
        Vector3 end = slot.transform.position;

        while (factor < 1)
        {
            factor += speed * Time.deltaTime;

            transform.position = Vector3.Lerp(start, end, factor);

            if (factor >= 1)
            {
                transform.position = end;


                SendDust();
                transform.localScale = size;
                this.GetComponent<SpriteRenderer>().sortingOrder = 1;
                info.transform.parent.GetComponent<Canvas>().sortingOrder = 2;


            }

            yield return null;
        }

        currentSlot = slot;
        PlayerInput.occupied = false;
        // slot.SetBook(this);
    }



    public Sprite GetColor()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
    public int GetID()
    {
        return this.id;
    }

    public void SetBook()
    {

        Enums.GameMode gameMode = Shelf.Instance.GetGameMode();
        this.id = Shelf.Instance.CreateId();


        if(gameMode == Enums.GameMode.color)
        {
            info.gameObject.SetActive(false);
        }
        if (gameMode == Enums.GameMode.letter)
        {
            info.gameObject.SetActive(true);
            info.text = Utils.GetLetter(id);
        }
        else
        {
            info.gameObject.SetActive(true);
            info.text = id.ToString();
        }


        Sprite s = Shelf.Instance.GetColor(this);
        this.GetComponent<SpriteRenderer>().sprite = s;
    }


    public void SetTextColor(int n)
    {
        info.color = textColors[n];
        dustColor = info.color;
        this.GetComponent<TrailRenderer>().startColor = info.color;
    }
}


