using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private bool isEmpty;
    [SerializeField] Book bookPrefab;

    Slot nextSlot;
    Slot previousSlot;
    Book currentBook;

    private void Start()
    {
      //  SetNewBook();


    }
    public bool IsEmpty()
    {
        return isEmpty;
    }

    public void SetPreviousSlot(Slot previous)
    {
        previousSlot = previous;
    }

    public void SetNextSlot(Slot next)
    {
        nextSlot = next;
    }
    public Book GetBook()
    {
        Book book = currentBook;
        currentBook = null;
        return book;
    }

    public Book GetCurrentBook()
    {
        return currentBook;
    }

    public void SetNewBook(bool toRight, Book newBook, bool definitive)
    {
        if (currentBook == newBook) return;
        Book oldBook = currentBook;
        currentBook = null;

        newBook.SetSlot(this);
        if (definitive)
        {
            currentBook = newBook;
            currentBook.transform.position = this.transform.position;
        }
           

        if (oldBook == null) return;

        Slot neighboorSlot = toRight ? nextSlot : previousSlot;

        if (neighboorSlot == null) return;

        neighboorSlot.SetNewBook(toRight, oldBook, true);

    }

    public void SetNewBook()
    {
        currentBook = Instantiate(bookPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
        currentBook.transform.localScale = this.transform.localScale;
        currentBook.SetBook();
    }


}
