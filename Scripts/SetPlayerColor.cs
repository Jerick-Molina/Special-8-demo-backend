using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetPlayerColor : MonoBehaviour
{


    public string myColor;
    public TextMeshProUGUI textUI;
    Sprite[] CardColors;

   
    
    // Start is called before the first frame update
   
    
    
    public void SetTeamColor(string  color, string name)
    {

        this.name = name;
        CardColors = Resources.LoadAll<Sprite>($"GameLobby/TeamCards/Teams");


        for (int i = 0; i <= CardColors.Length -1; i++)
        {


            
            if(CardColors[i].name == color)
            {

                this.GetComponent<SpriteRenderer>().sprite = CardColors[i];

            }
        }

        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
