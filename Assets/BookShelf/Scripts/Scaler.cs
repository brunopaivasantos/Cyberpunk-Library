using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] float height;
    // Start is called before the first frame update
    void Start()
    {
        SetScale();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void SetScale()
    {
        float width = ScreenSize.GetScreenToWorldWidth;
        //float height = ScreenSize.GetScreenToWorldHeight;


        transform.localScale = new Vector3(width, height);
        transform.position = Vector3.zero;
    }
}
