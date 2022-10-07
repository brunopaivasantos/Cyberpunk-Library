using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;
   // [SerializeField, Range(0f, 1f)] private float intervalThreshold = .2f;
    //[SerializeField, Range(0f, 1f)] private float distanceThreshold = .1f;

    // [SerializeField] private GameObject trail;

    private InputManager inputManager;

    private Vector2 startPosition;
    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine coroutine;
    private bool touching;
    private Book book;
    private float time;
    private Transform currentSlot;
    public static Action<float> SwipeForce;

    public static bool occupied;
    private void Awake()
    {
        inputManager = InputManager.Instance;
        occupied = false;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += StartTouch;
        inputManager.OnEndTouch += EndTouch;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= StartTouch;
        inputManager.OnEndTouch -= EndTouch;
    }

    private void Update()
    {
        if (!touching) return;
        if (Shelf.gameOver) return;
        currentPosition = inputManager.PrimaryPosition();

        if (book != null)
        {
            book.SetPosition(currentPosition);
            currentSlot = GetSlot();


            if (currentSlot == null || time < .025f) return;
            Shelf.Instance.ChangeLine(currentSlot.GetComponent<Slot>(), book, false);
        }
        //if (Timer.GetTime() > intervalThreshold)
        //{

        //    if (Vector2.Distance(currentPosition, lastPosition) <= distanceThreshold)
        //    {
        //        startPosition = lastPosition;
        //    }

        //    lastPosition = inputManager.PrimaryPosition();
        //}
    }

    private Transform GetSlot()
    {
        Transform slot = Utils.TouchSlot(currentPosition);
        if (currentSlot != null && currentSlot.Equals(slot))
        {
            time += Time.deltaTime;
        }
        else
            time = 0;
        return slot;
    }
    private void StartTouch(Vector2 position, float time)
    {
        if (occupied) return;
        startPosition = position;
        startTime = time;
        touching = true;
        Transform slot = Utils.TouchSlot(position);
        Book book = null;

        if (slot != null)
        {
            book = slot.GetComponent<Slot>().GetBook();
            Shelf.Instance.SetLastIndex(slot.GetComponent<Slot>());
        }


        if (book != null)
        {
            this.book = book;
            this.book.SetHands(true);
        }
        //  coroutine = StartCoroutine(Trail());
    }

    //private IEnumerator Trail()
    //{
    //    while (true)
    //    {
    //        trail.transform.position = inputManager.PrimaryPosition();
    //        yield return null;
    //    }
    //}
    private void EndTouch(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        touching = false;
        if (book == null) return;
        //if (book != null)
        //        book.SetHands(false);
        Transform slot = Utils.TouchSlot(position);
        if (slot == null)
        {
            book.Return();
            book.SetHands(false);
            Shelf.Instance.CheckVictory();

            return;
        }
        //if (book == null || slot == null) return;

        Shelf.Instance.ChangeLine(slot.GetComponent<Slot>(), book, true);
        this.book.SendDust();
        this.book = null;
        //  slot.GetComponent<Slot>().SetNewBook();
        //book.SetNewSlot(slot.GetComponent<Slot>());
        Shelf.Instance.CheckVictory();
        //return;
        //book.SetHands(false);
        //book.Return();
        //book = null;
        //trail.SetActive(false);
        //    StopCoroutine(coroutine);
    }


    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
                 endTime - startTime <= maximumTime)
        {
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            SwipeForce?.Invoke(4f);
            // Debug.Log("swipe up");
        }

        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            // Debug.Log("swipe down");
        }

        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            // Debug.Log("swipe Right");
        }

        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            // Debug.Log("swipe left");
        }
    }
}
