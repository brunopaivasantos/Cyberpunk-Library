using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Mode", menuName = "Game Mode")]
public class GameMode : ScriptableObject
{
    public Enums.GameMode gameMode;
    public int colorsQuantity;
    public int standQuantity;
    public int idQuantity;
    public int minId;
    public int maxId;
    
}
