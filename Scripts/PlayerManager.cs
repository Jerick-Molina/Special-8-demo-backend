using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    //GameSettings
    public Transform[] playerCards;
    public TextMeshProUGUI textUI;
    public Transform setTeamColor;
    public Transform yourColor;

    public bool isPlayerTurn = false;
    public string username;
    public string teamColor;
    public int number;
  
    void Awake()
    {
      //  DontDestroyOnLoad(this);    
    }
    //Lobby
    void Start()

    {

        this.name = username;
        this.transform.parent = GameObject.Find("Players").transform;
        setTeamColor = GameObject.Find("NoTeam").transform; 

        setTeamColor.GetComponent<SetPlayerColor>().SetTeamColor(teamColor, username);
    }

    public void LateUpdate()
    {
        
    }

    public void UpdateCards()
    {

    }
}
