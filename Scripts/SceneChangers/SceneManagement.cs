using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{

    WebSocketManager ws;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void StartGame()
    {
       
        SceneManager.LoadScene(1);
    }
}
