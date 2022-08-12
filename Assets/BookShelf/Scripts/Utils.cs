using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static Transform TouchSomething(Vector3 position)
    {
        position.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector3.forward);
        if (hit.collider == null) return null;
        if (hit.collider.tag != "Book") return null;
        return hit.collider.transform;
        
    }

    public static Transform TouchSlot(Vector3 position)
    {
        position.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector3.forward);
        if (hit.collider == null) return null;
        if (hit.collider.tag != "Slot") return null;
        return hit.collider.transform;

    }
    

    public static string RomanAlgorithm(int number)
    {
        Dictionary<int, string> mapValues = new Dictionary<int, string>();
        mapValues.Add(90, "XC");
        mapValues.Add(50, "L");
        mapValues.Add(40, "XL");
        mapValues.Add(10, "X");
        mapValues.Add(9, "IX");
        mapValues.Add(5, "V");
        mapValues.Add(4, "IV");
        mapValues.Add(1, "I");

        List<int> baseValues = new List<int>{ 90, 50, 40, 10, 9, 5, 4, 1};
        int baseValue = 90;
        string result = "";
        int remainder = 100;
        int index = 0;
        while(remainder > 0)
        {
            if(number >= baseValue)
            {
                int quotient = number / baseValue;
                for (int i = 0; i < quotient; i++)
                {
                    result += mapValues[baseValue];
                }
                
                number = number % baseValue;
            }
            else
            {
                index++;
                baseValue = baseValues[index];
            }
            remainder = number;
        }

        Debug.Log(result);
        return result;
      
    }

    public static string GetLetter(int id)
    {
        char letter = (char)id;
        return letter.ToString();
    }
}
