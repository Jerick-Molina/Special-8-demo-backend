using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerPlatform : MonoBehaviour
{

    //gameSettings
    public CardBluePrint_SO[] playerCards;
    private GameManager gm;
    //platformSettings
     TextMeshProUGUI pName;
     Sprite[] pPlatForm;
    public string username;
    public string teamColor;
    public int playerNumber;


    public Transform CardHands;
    public Transform extras;
    public bool isPlayerTurn;

    public Transform Players;
    public Transform player;
    bool didPlayOnce = false;
    // Start is called before the first frame update
    void Start()
    {

        
        extras = this.transform.GetChild(0);
        Players = GameObject.Find("Players").transform;
        CardHands = this.gameObject.transform.GetChild(1);
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    private void LateUpdate()
    {
        DidPlayerWin();
        SetPlatformSettings();
       
    }
    public void DidPlayerWin()
    {
        if (CardHands.childCount == 0 && this.name != "Team")
        {
            if (didPlayOnce == false)
            {
                gm.OnPlayerWin(username);

                didPlayOnce = true;
            }
        }
    }
    public void SetPlatformSettings()
    {
        if (username != "Empty")
        {
            pPlatForm = Resources.LoadAll<Sprite>($"PlayerPads/{teamColor}R");
            extras.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = username;
            this.name = $"{username}";
          
        }
        else
        {
            extras.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = username;
        }
       
    }

   

}
