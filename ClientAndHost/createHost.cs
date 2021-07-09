
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class createHost 
{

    public string roomCode { get; set; }

    public string method { get; set; }

    public parameters @params;
    public  class parameters {
        public int maxPlayers { get; set; }
        public int currentPlayers { get; set; }
    } 

}
