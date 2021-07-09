using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonClasses : MonoBehaviour
{
    /*
     *  public class "Class"
     *  {
     *      public string method {get;set}
     *      
     *      public string submethod {get;set;}
     *      
     *      public Values values;
     *      
     *      public class Values{
     *      
     *      }
     * }
     */



    
    public class jsonBluePrints
    {

        public string methods { get; set; }

        public string submethods { get; set; }

        public _values values;

        public class _values
        {
            public string Username { get; set; }
            public string RoomCode { get; set; }
            // GameValues
            public string MainColor { get; set; }
            public string SubColor { get; set; }

            //CardValues
            public string CardColor { get; set; }
            public int CardId { get; set; }
            //GameConfig
            public string MaxPlayers { get; set; }
            public int CurrentPlayers { get; set; }

        }
    }
}
