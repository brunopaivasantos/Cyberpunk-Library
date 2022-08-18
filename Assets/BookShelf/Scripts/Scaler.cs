using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] bool background;
    // Start is called before the first frame update
    void Start()
    {
        if (background)
            SetScale();
        else
            NewSetScale();
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

    public void NewSetScale()
    {
        //setar o tamanho (altura da imagem) do Shelf;

        float height = ScreenSize.GetScreenToWorldHeight;
        Vector3 position = new Vector3(0, ScreenSize.GetScreenToWorldHeight / 2, 0);
        //float height = ScreenSize.GetScreenToWorldHeight;


        transform.localScale = new Vector3(height + .4f, height + .4f);
        transform.position = position;
    }
}
