using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using static JsonClasses;


public class WebSocketManager : MonoBehaviour
{

    WebSocket ws;

    //    Dictionary<string, string> ClientConnected = new Dictionary<string, string>();


    TeamColorSetter colorSetter;
    GameManager gm;
    LobbyManager lobbyManager;
    public int playerNumber = 0;
    public string GameRoomCode = "ABCD";



    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
        ws = new WebSocket(new Uri("htt://localhost:8080/"));
        ws.OnMessage += OnMessageReceived;
        ws.OnOpen += OnWebSocketOpen;
        ws.Open();
       
        colorSetter = GameObject.Find("Players").GetComponent<TeamColorSetter>();
        colorSetter.SetRoomCode("ABCD");
    }
    private void OnWebSocketOpen(WebSocket webSocket)
    {
        Debug.Log($"WebSocketOpen! ");
        ConnectedToserver();
    }

    private void OnMessageReceived(WebSocket webSocket, string message)
    {
        jsonBluePrints mReceived = JsonConvert.DeserializeObject<jsonBluePrints>(message);
        Debug.Log(mReceived.methods);
        var MainMethods = mReceived.methods;
        var Submethods = mReceived.submethods;
        var Values = mReceived.values;
        switch (MainMethods)
        {
            case "Game":

                switch (Submethods)
                {
                    case "OnPlayerConnection":

                        Debug.Log($"Got it {Submethods}");
                        colorSetter.SetTeamColor(Values.Username);
                        break;

                    case "PlacingCard":
                        Debug.Log($"Got it {Submethods}");
                        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                        if (Values.CardColor != null)
                        {
                            gm.PlayerPlacedCard(Values.CardColor, Values.CardId);
                        }

                        break;

                    case "PlayerWantsCard":
                        Debug.Log($"Got it {Submethods}");
                        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

                        gm.GenerateCardToPlayer(1,true);

                        break;
                }


                break;

            case "Reconnect":

                break;

        };
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectToServer()
    {
      //  ws.Open();
    }
    public void ConnectedToserver()
    {
        var json = new jsonBluePrints
        {
            methods = "Server",
            submethods = "CreateLobby",
            values = new jsonBluePrints._values
            {
                RoomCode = "ABCD",
            }
        };
        wsSend(json);
    }
    public  void StartGame()
    {
        var json = new jsonBluePrints
        {
            methods = "Game",
            submethods = "StartGame",
              values = new jsonBluePrints._values
              {    
                  RoomCode = "ABCD",
              }
        };
        
        wsSend(json);
    }
    public void SendPlayerColor(string username,string MColor, string SColor)
    {
        var json = new jsonBluePrints
        {
            methods = "Game",
            submethods = "SuccessConnection",

            values = new jsonBluePrints._values
            {
                Username = username,
                RoomCode = "ABCD",
                MainColor = MColor,
                SubColor = SColor
               
            }
        };
        wsSend(json);
    }
    public void SendCards(string username,string color, int number)
    {
        var json = new jsonBluePrints
        {
            methods = "Game",
            submethods = "ReceiveCards",

            values = new jsonBluePrints._values
            {
                Username = username,
                RoomCode = "ABCD",
                CardColor = color,
                CardId = number,
                
            }
        };
        wsSend(json);
    }
    public void NextTurn(string username)
    {
        var json = new jsonBluePrints
        {
            methods = "Game",
            submethods = "Player'sTurn",
            values = new jsonBluePrints._values
            {
                Username = username,
                RoomCode = "ABCD"
            }
        };
        wsSend(json);
    }
    public void NextCard(string cardColor, int cardId)
    {
        var json = new jsonBluePrints
        {
            methods = "Game",
            submethods = "CurrentCard",
            values = new jsonBluePrints._values
            {
                CardColor = cardColor,
                CardId = cardId,
                RoomCode = "ABCD"
            }
            
        };
        wsSend(json);
    }

    public void OnGameFinish()
    {
        var json = new jsonBluePrints
        {
            methods = "Game",
            submethods = "GameFinished",
            values = new jsonBluePrints._values
            {
               
                RoomCode = "ABCD"
            }

        };
        wsSend(json);
    }
    private void wsSend<T>(T json)
    {
        Debug.Log("Sent it");
        string _jConvert = JsonConvert.SerializeObject(json);

        if (_jConvert != null){
            ws.Send(_jConvert);
        }
    }
}
