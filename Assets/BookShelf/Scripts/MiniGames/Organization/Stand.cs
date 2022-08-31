using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    [SerializeField] Slot firstSlot;
    List<Slot> slots = new List<Slot>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBooks()
    {
        int total = slots.Count;
        Destroy(slots[0].GetCurrentBook().gameObject);
        for (int i = 1; i < total; i++)
        {
            Destroy(slots[i].GetCurrentBook().gameObject);
            Destroy(slots[i].gameObject);
        }
        slots.Clear();
    }
    public void SetBooks(bool fromTitleScreen)
    {
        Bounds topBound = this.GetComponent<SpriteRenderer>().bounds;
        float shelfWidth = topBound.extents.x * 2;
        float bookWidth = firstSlot.GetComponent<Collider2D>().bounds.extents.x * 2;
        int slotsQuantity = (int)(shelfWidth / bookWidth);
        Vector3 nextPos = firstSlot.transform.position + Vector3.right * bookWidth;
        slots.Add(firstSlot);

        Shelf.Instance.AddSlot(firstSlot);
        bool redColor = false;
        Color c = Color.green;
        slots[0].SetNewBook();
        for (int i = 1; i<= slotsQuantity; i++)
        {
            Slot s = Instantiate(firstSlot, nextPos, Quaternion.identity, this.transform);
            nextPos = s.transform.position + Vector3.right * bookWidth;
            Debug.DrawLine(s.transform.position, nextPos, c, 50f);
            redColor = !redColor;


            c = redColor ? Color.red : Color.green;
            
            s.SetNewBook();
            slots.Add(s);
            s.SetPreviousSlot(slots[i - 1]);
            slots[i - 1].SetNextSlot(s);
            Shelf.Instance.AddSlot(s);
        }

        
    }

    public List<Slot> GetSlots()
    {
        return slots;
    }
}
