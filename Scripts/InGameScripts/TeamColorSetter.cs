using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Lang;
using static JsonClasses;

public class TeamColorSetter : MonoBehaviour
{

    public PlayerManager newPlayer;
     WebSocketManager ws;
    private int playerNumber = 0;
    Transform startGameButton;
    Dictionary<string, string> mainColor = new Dictionary<string, string>() {
        {"Red","#E31300"},{"Maroon","#590F15"},
        {"Blue","#3642E3"},{"Pink","#E3178E"},
        {"Green","#0D9900"},{"Orange","#E07A21"},
        {"LightBlue","#2BE3D4"},{"Purple","#BB44E3"},
    };
    Dictionary<string, string> subColor = new Dictionary<string, string>() {
        {"Red","#990E00"},{"Maroon","#400B0F"},
        {"Blue","#242C96"},{"Pink","#990F60"},
        {"Green","#075700"},{"Orange ","#9E5618"},
        {"LightBlue","#1D998F"},{"Purple","#7E2E99"},
    } ;


    List<string> teamColors = new List<string>() { "Red", "Blue", "Maroon","Orange", "Pink","Green", "LightBlue", "Purple"};
     
     
private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        ws = GameObject.Find("ServerManager").GetComponent<WebSocketManager>();
        startGameButton = GameObject.Find("StartButton").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(startGameButton != null)
        {
            if (this.transform.childCount > 0)
            {

                startGameButton.GetComponent<Button>().interactable = true;
            }
        }
    }
    //Teams SOON ADDED
    public void SetRoomCode(string roomCode)
    {
        GameObject.Find("RoomCode").GetComponent<TextMeshProUGUI>().text += roomCode;
       

    }
    public void SetTeamColor(string username)
    {
        string typeOfColor = teamColors[UnityEngine.Random.Range(0, teamColors.Count)];
        

        foreach(var mColor in mainColor)
        {
           
            if(mColor.Key == typeOfColor)
            {
                foreach(var sColor in subColor)
                {

                    if(sColor.Key == typeOfColor)
                    {
                        
                        newPlayer.teamColor = typeOfColor;
                        newPlayer.username = username;
                        newPlayer.number = playerNumber;
                        playerNumber++;
                        ws.SendPlayerColor(username, mColor.Value, sColor.Value);

                        Instantiate(newPlayer);
                        teamColors.Remove(typeOfColor);
                    }
                }
            }
        }
        Debug.Log(this.transform.childCount);

       
    } 

  
} 
