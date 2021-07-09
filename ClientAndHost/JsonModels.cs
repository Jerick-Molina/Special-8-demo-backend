using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SignalRCore;
using System.Dynamic;


//Recieve information from client
public class ClientToHost
    {
        public string username { get; set; }

        public string method { get; set; }

        public clientParam @params;

           public class clientParam
            {
            public string message;
            }
    }
    
    //Sends information back
    public class HostToClient
    {
        public string username { get; set; }
        public string method { get; set; }

        public hostParams @params;

        public class hostParams
        {
            public string message;
        }
    }


public class FirstPlayer
{
    public string username { get; set; }
    public string roomCode { get; set; }
    public string method { get; set; }
}

//playercolor

public class playerColor
{
    public string method { get; set; }
    public string roomCode { get; set; }
    public string color { get; set; }
    public string username { get; set; }
}
//sendCards

  public class SendCards
{
    public string username { get; set; }
    public string roomCode { get; set; }
    public string method { get; set; }
}

//Receive Cards
public class ReceiveCards
{
    public string method { get; set; }
    public string username { get; set; }
    public string roomCode { get; set; }
    public cardParams @params { get; set; }
    public class cardParams
    {
        public string Color { get; set; }
        public int Number { get; set; }
    }
}
// Errror that happens
 public class ErrorCodes
{
   public string username { get; set; }
    public  string roomcode { get; set; }
    public string method { get; set; }
    public errorParams @params;

    public class errorParams
    {
       
    }
}

//starting game 
public class gameready
{
    public string method { get; set; }
}

//Client to Host JSON
public class PlacedCard
{
    public string username { get; set; }
    public string cardcolor { get; set; }
    public int cardId { get;set; }

}


//The person chosen to start first 


public class NextTurn
{
    public string username { get; set; }
    public string method { get; set; }

    public string color { get; set; }
    public int cardId { get; set; }
}
//Host  to Client JSON