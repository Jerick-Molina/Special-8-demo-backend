using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "NewCard", menuName = "ShiftCard")]
public class CardBluePrint_SO : ScriptableObject
{
    public string CardColor;
    public int CardId;
    public string hiddenCard;
}
