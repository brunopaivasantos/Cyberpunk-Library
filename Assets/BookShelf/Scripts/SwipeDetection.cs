using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    [SerializeField] private GameObject trail;
    [SerializeField] bool isGetToPageGame;
    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine coroutine;

    public static Action<float> SwipeForce;
    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {        
        startPosition = position;
        startTime = time;

        //trail.SetActive(true);
        //trail.transform.position = position;
       // coroutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail()
    {
        while(true)
        {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    //    trail.SetActive(false);
     //   StopCoroutine(coroutine);
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
           // SwipeForce?.Invoke(4f);
        }

        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            //Debug.Log("swipe down");
        }

        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
           
        }

        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            
        }
    }
}
