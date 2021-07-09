using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetter : MonoBehaviour
{
    public Transform players;
    public Transform spawnPoints;
    List<GameObject> AvailablePlayers = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
      
        players = GameObject.Find("Players").transform;
        spawnPoints = GameObject.Find("SpawnPoints").transform;
        CountPlayers();
        SetTeam();
    }

    
    public void CountPlayers()
    {
        for (var c = 0; c < players.childCount; c++)
        {
            AvailablePlayers.Add(players.GetChild(c).gameObject);
        }
    }

    private void LateUpdate()
    {
     
    }
    public void SetTeam()
    {
        var spawnpointnumber = 0;
        foreach (var player in AvailablePlayers)
        {
            spawnPoints.transform.GetChild(spawnpointnumber).GetComponent<PlayerPlatform>().username = player.transform.GetComponent<PlayerManager>().username;
            spawnPoints.transform.GetChild(spawnpointnumber).GetComponent<PlayerPlatform>().teamColor = player.transform.GetComponent<PlayerManager>().teamColor;
            spawnPoints.transform.GetChild(spawnpointnumber).GetComponent<PlayerPlatform>().playerNumber = spawnpointnumber;

            spawnpointnumber++;
        }
    }
}
