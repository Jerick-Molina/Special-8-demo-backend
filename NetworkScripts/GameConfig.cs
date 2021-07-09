using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.WebSocket;
using BestHTTP;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

public class GameConfig : MonoBehaviour
{
    // Start is called before the first frame update
    WebSocket wSocket;
  
    Dictionary<string,string> ClientsConnected = new Dictionary<string, string>();
    Dictionary<string, string> GameVip = new Dictionary<string, string>();
    List<string> AvailableTeamColors = new List<string>() { "Red","LightBlue","Pink","Maroon","White","Green","Purpl    e","Blue"};
    List<string> usedTeamColors = new List<string>();
     GameManager gameManager = new GameManager();
    ClientToHost cModel = new ClientToHost();
    HostToClient hModel = new HostToClient();
    public PlayerManager newPlayer;
    public int playerNumber = 0;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public string userName;
    public string roomCode;

    private void Start()
    {
        wSocket = new WebSocket(new Uri("htt://localhost:8080/"));
        wSocket.OnMessage += OnMessageReceived;
        wSocket.OnOpen += OnWebSocketOpen;

    }
    private void OnApplicationQuit()
    {
        wSocket.Close();
        Debug.Log("Socket closed");
    }
    private void OnWebSocketOpen(WebSocket wSocket)
    {
        Debug.Log($"WebSocketOpen! ");
        Debug.Log(wSocket.IsOpen);
        IsConnectedToServer();
    }

    private void OnMessageReceived(WebSocket webSocket, string message)
    {

        ErrorCodes messageRecieved = JsonConvert.DeserializeObject<ErrorCodes>(message);
        PlacedCard placedCard = JsonConvert.DeserializeObject<PlacedCard>(message);
        switch (messageRecieved.method)
        {
            case "Connected":
              
                if (messageRecieved.username != null)
                {
                    string typeColor = AvailableTeamColors[UnityEngine.Random.Range(0, AvailableTeamColors.Count)];

                    Debug.Log($" {messageRecieved.username} has joined : {messageRecieved.roomcode}");
                    ClientsConnected.Add(messageRecieved.username, messageRecieved.roomcode);

                    if (ClientsConnected.Count == 1)
                    {
                        Debug.Log("You joined! First");
                        GameVip.Add(messageRecieved.username, messageRecieved.roomcode);
                      
                        FirstPlayerIngame(messageRecieved.username, messageRecieved.roomcode);
                    }
                    else if (ClientsConnected.Count == 3)
                    {
                        foreach (var vip in GameVip)
                        {
                            GameReady(vip.Key, vip.Value);
                        }
                    }
                    else
                    {
                     
                    }
                    userName = messageRecieved.username.ToString();
                    roomCode = messageRecieved.roomcode.ToString();
                    foreach (var color in AvailableTeamColors)
                    {
                        if (color == typeColor)
                        {

                            newPlayer.teamColor = color;

                        }
                    }
                    newPlayer.number = playerNumber;
                    playerNumber++;
                  
                    newPlayer.username = userName;
                    givePlayerColor(newPlayer.username, messageRecieved.roomcode, newPlayer.teamColor);
                    Instantiate(newPlayer);
                    AvailableTeamColors.Remove(typeColor);
                }
                break;
            case "gameready":

             
                SceneManagement scne = new SceneManagement();

                scne.StartGame();
                StartGame();
            
                break;

            case "placedcard":
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                if (placedCard.cardcolor != null )
                {
                  
                    Debug.Log($" {placedCard.cardcolor}, {placedCard.cardId}");
                //    gameManager.PlayerPlacedCard(placedCard.cardcolor, placedCard.cardId);
                    

                }
                break;

            case "giveCard":
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                Debug.Log("hello");
             //   gameManager.PlayerWantsCard();


                break;
        };



    }

   
    public void ConnectToServer()
    {
        wSocket.Open();
    }

    public void StartGame()
    {
        var _json = new gameready
        {
            method = "startgame",
        };
        string _jConvert = JsonConvert.SerializeObject(_json);
        if (_jConvert != null)
        {
            wSocket.Send(_jConvert);
        }
    }
    //seperate new card and ready game
    public void GiveCards(string user, string roomCodes, string  color,int number)
    {
        

        var json = new ReceiveCards
        {
           method = "newcard",
           roomCode = roomCodes,
           username = user,
           @params  = new ReceiveCards.cardParams { Color =  color, Number = number}
        };
        string jConvert = JsonConvert.SerializeObject(json);
        if (jConvert != null)
        {
          
            wSocket.Send(jConvert);
        }


    }
    public void  GameReady(string user, string roomCodes)
    {
        var json = new FirstPlayer
        {
            roomCode = roomCodes,
            username = user,
            method = "startgamebutton",
        };
        string jConvert = JsonConvert.SerializeObject(json);
        if (jConvert != null)
        {
            
            wSocket.Send(jConvert);
        }

    }
    public void givePlayerColor(string user,string roomCodes,string color)
    {
        var json = new playerColor
        {
            roomCode = roomCodes,
            username = user,
            color = color,
            method = "getcolor"
            
        };
        string jConvert = JsonConvert.SerializeObject(json);
        if (jConvert != null)
        {
          
            wSocket.Send(jConvert);
        }
    }
    // Update is called once per frame
    public void FirstPlayerIngame(string user,string roomCodes)
    {
        var json = new FirstPlayer
        {
            roomCode = roomCodes,
            username = user,
            method = "firstplayer",
        };
        string jConvert = JsonConvert.SerializeObject(json);
        if(jConvert != null)
        {
            
            wSocket.Send(jConvert);
        }
    }
   public void PlayerStartsFirst(string user)
    {
        var json = new NextTurn
        {
            username = user,
            method = "YourTurn",
        };

        string jConvert = JsonConvert.SerializeObject(json);
        if (json != null)
        {
            wSocket.Send(jConvert);
        }
    }
    public void newCard(string color, int cardId)
    {
        var json = new NextTurn
        {
            color = color,
            cardId = cardId,
            method = "currentCard",
        };

        string jConvert = JsonConvert.SerializeObject(json);
        if (json != null)
        {
            wSocket.Send(jConvert);
        }
    }
    public void NextTurn(string user)
    {
        var json = new NextTurn
        {
            username = user,
            method = "YourTurn",
        };

        string jConvert = JsonConvert.SerializeObject(json);
        if (json != null)
        {
            wSocket.Send(jConvert);
        }
    }
    public void IsConnectedToServer()
    {
        
        
        Debug.Log(wSocket.IsOpen);
        if (wSocket.IsOpen == true)
        {
            var thejson = new createHost
            {
                roomCode = "ABCD",
                method = "createhost",
                @params = new createHost.parameters { maxPlayers = 6, currentPlayers = 0 }

            };
            string json = JsonConvert.SerializeObject(thejson);
            if (json != null)
            {
             
                wSocket.Send(json);
            }
        }
    }
}
