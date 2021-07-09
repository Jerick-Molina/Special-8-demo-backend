using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    WebSocketManager ws;
    TableManagement TManagement;
    OnSelectedColorScript selectedColorScript;
    CardGenerator cG;

    bool didGoP1 = false;

    public ScriptableObject[] Cards;
    public CardBluePrint_SO[] coloredCards;
    public CardBluePrint_SO[] specialCards;
    public CardBluePrint_SO[] reallySpecialCards;
    public CardBluePrint_SO chosenCards;

    public CardsExample InsCard;

    public string currentCardColor;

    //GameModes;
    //Normal 
    public bool NormalGameMode = true;
    //Teams
    // public bool TeamGameMode = false;
    //VerySpecial
    // public bool SpecialGameMode = false;


    private int currentPlayerTurn;
    public int currentCardId;
    public int TotalCards = 7;

    private bool isClockWise = true;

    public Transform players;
    public Transform parent;
    public Transform teams;
    public Transform ShowTurn;

    public CardsExample card;


    //AvailableCards'
   public List<Sprite> AvailableCards = new List<Sprite>();

    //OnGoingCards;
   public List<Sprite> OnGoingCards = new List<Sprite>();
    List<Transform> AvailablePlayers = new List<Transform>();

    string[] separators = { "_" };
    public string[] words = { };
    // Start is called before the first frame update
    void Start()
    {
        cG = this.GetComponent<CardGenerator>();
        cG._CardGenerator(false);
        ws = GameObject.Find("ServerManager").GetComponent<WebSocketManager>();
        players = GameObject.Find("Players").transform;
        teams = GameObject.Find("SpawnPoints").transform;
        TManagement = GameObject.Find("TableManagement").GetComponent<TableManagement>();
        selectedColorScript = GameObject.Find("ColorPlaced").GetComponent<OnSelectedColorScript>();
        GetAllPlayers();
        OnGameStart();
    }
    public void PlayerPlacedCard(string cardColor, int cardId)
    {
        //deleteCard


        //Card is Placed
        TManagement.UpdatePlatformAndCard(cardColor, cardId);

        //Check if it a specialCard,

        DeletePlayerCard(cardColor, cardId);
        TypeOfCard(cardColor, cardId);
        ws.NextCard(cardColor, cardId);

        //SendNewCardRequirements to The wesbite




    }
    public void transferCards(List<Sprite> cards)
    {
        AvailableCards = cards;
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetAllPlayers()
    {
        for (var i = 0; i < players.childCount; i++)
        {
            AvailablePlayers.Add(players.GetChild(i));
        }

    }

    

    // Add Rarerity Chances  as in 10% chance of getting a black
    void OnGameStart()
    {
        //Make Giving Player Cards its own function but check if any of the Bools modes for the actual game. Activate other classes.
        ws.StartGame();
        GivingPlayersCards();
        FirstRandomCard();
        StartFirstPlayer();
    }

    void GivingPlayersCards()
    {

        for (int i = 0; i <= TotalCards - 1; i++)
        {

            parent = GameObject.Find("CardSpawnpoint").transform;
            foreach (var player in AvailablePlayers)
            {



                
                parent = teams.GetChild(player.GetComponent<PlayerManager>().number).GetChild(1);
               



                var chosenCardNumber = UnityEngine.Random.Range(0, AvailableCards.Count);
                
                //  Debug.Log(chosenCardNumber);
                words = AvailableCards[chosenCardNumber].name.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                // Debug.Log(words[0]);

                if (words[0] != "Wild")
                {

                    if (int.Parse(words[1]) == 0)
                    {

                        card.GetComponent<CardsExample>().Color = words[0];
                        card.GetComponent<CardsExample>().number = (int.Parse(words[1]));
                        ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                        //    Instantiate(card, parent);
                        OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                        AvailableCards.RemoveAt(chosenCardNumber);
                    }
                    else if (int.Parse(words[1]) != 0)
                    {
                        card.GetComponent<CardsExample>().Color = words[0];
                        card.GetComponent<CardsExample>().number = (int.Parse(words[1]));
                        //

                        if (OnGoingCards.Count != 0)
                        {
                            for (int c = 0; c <= OnGoingCards.Count - 1; c++)
                            {

                                if (OnGoingCards[c] == AvailableCards[chosenCardNumber])
                                {

                                    AvailableCards.RemoveAt(chosenCardNumber);
                                    ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                                    //   Instantiate(card, parent);
                                    didGoP1 = true;
                                    break;
                                }
                                else
                                {
                                    didGoP1 = false;
                                }

                            }
                            if (didGoP1 == false)
                            {

                                OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                                ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                                //    Instantiate(card, parent);

                            }
                        }
                        else
                        {
                            OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                            ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                            //  Instantiate(card, parent);
                        }
                        //
                    }

                }
                else if (words[0] == "Wild")
                {


                    card.GetComponent<CardsExample>().Color = words[0];
                    card.GetComponent<CardsExample>().number = (int.Parse(words[1]));
                    ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                    //  Instantiate(card, parent);

                }

                 //   card.GetComponent<CardsExample>().Owner = player.name;

                Instantiate(card, parent);
            }

            Debug.Log("Hi");
            
        }
        FirstRandomCard();
    }


    void FirstRandomCard()
    {

        //  Debug.Log(chosenCardNumber);
        var chosenCardNumber = UnityEngine.Random.Range(0, AvailableCards.Count);
        chosenCardNumber = UnityEngine.Random.Range(0, AvailableCards.Count); 
        words = AvailableCards[chosenCardNumber].name.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        // Debug.Log(words[0]);
        Debug.Log(int.Parse(words[1]));
        if (int.Parse(words[1])  < 9)
        {
           

            if (words[0] != "Wild")
            {

                if (int.Parse(words[1]) == 0)
                {

                    card.GetComponent<CardsExample>().Color = words[0];
                    card.GetComponent<CardsExample>().number = (int.Parse(words[1]));

                    //    Instantiate(card, parent);
                    OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                    AvailableCards.RemoveAt(chosenCardNumber);
                }
                else if (int.Parse(words[1]) != 0)
                {
                    card.GetComponent<CardsExample>().Color = words[0];
                    card.GetComponent<CardsExample>().number = (int.Parse(words[1]));
                    //

                    if (OnGoingCards.Count != 0)
                    {
                        for (int c = 0; c <= OnGoingCards.Count - 1; c++)
                        {

                            if (OnGoingCards[c] == AvailableCards[chosenCardNumber])
                            {

                                AvailableCards.RemoveAt(chosenCardNumber);

                                //   Instantiate(card, parent);
                                didGoP1 = true;
                                break;
                            }
                            else
                            {
                                didGoP1 = false;
                            }

                        }
                        if (didGoP1 == false)
                        {

                            OnGoingCards.Add(AvailableCards[chosenCardNumber]);

                            //    Instantiate(card, parent);

                        }
                    }
                    else
                    {
                        OnGoingCards.Add(AvailableCards[chosenCardNumber]);

                        //  Instantiate(card, parent);
                    }
                    //
                }


            }
            else
            {
                FirstRandomCard();
            }
            TManagement.UpdatePlatformAndCard(words[0], int.Parse(words[1]));
            ws.NextCard(words[0], int.Parse(words[1]));
            //  Instantiate(card, parent);
        }
        else
        {
            Debug.Log("Hello!"); 
        }
       
    }

    void StartFirstPlayer()
    {
        int i = 0;
        var chosen = UnityEngine.Random.Range(0, AvailablePlayers.Count);

        foreach (var player in AvailablePlayers)
        {
            if (chosen == i)
            {




                currentPlayerTurn = chosen;

                ShowTurn = teams.GetChild(i);

                ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(1).gameObject.SetActive(true);
                ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(1).GetComponent<ParticleSystem>().Play();
                ws.NextTurn(player.GetComponent<PlayerManager>().username);

                Debug.Log(player.GetComponent<PlayerManager>().username + "'s Turn");

                i++;
            }
            else
            {
                i++;
            }
        }

        i = 0;
    }

    public void GenerateCardToPlayer(int amount, bool playerButton)
    {
        bool didItComeFromPlayer = playerButton;


        for (var am = 0; am <= amount - 1; am++)
        {
            int i = 0;
            foreach (var player in AvailablePlayers)
            {



                parent = teams.GetChild(player.GetComponent<PlayerManager>().number).GetChild(1);
                

                if (i == currentPlayerTurn)
                {

                    var chosenCardNumber = UnityEngine.Random.Range(0, AvailableCards.Count);
                    //parent = GameObject.Find("CardSpawnPoint").transform;
                    Debug.Log(parent);
                    words = AvailableCards[chosenCardNumber].name.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    // Debug.Log(words[0]);

                    if (words[0] != "Wild")
                    {

                        if (int.Parse(words[1]) == 0)
                        {

                            card.GetComponent<CardsExample>().Color = words[0];
                            card.GetComponent<CardsExample>().number = (int.Parse(words[1]));
                            ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                            //   Instantiate(card, parent);
                            OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                            AvailableCards.RemoveAt(chosenCardNumber);
                        }
                        else if (int.Parse(words[1]) != 0)
                        {
                            card.GetComponent<CardsExample>().Color = words[0];
                            card.GetComponent<CardsExample>().number = (int.Parse(words[1]));

                            if (OnGoingCards.Count != 0)
                            {
                                for (int c = 0; c <= OnGoingCards.Count - 1; c++)
                                {

                                    if (OnGoingCards[c] == AvailableCards[chosenCardNumber])
                                    {

                                        AvailableCards.RemoveAt(chosenCardNumber);
                                        ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                                        //  Instantiate(card, parent);
                                        didGoP1 = true;
                                        break;
                                    }
                                    else
                                    {
                                        didGoP1 = false;
                                    }

                                }
                                if (didGoP1 == false)
                                {

                                    OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                                    ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                                    //  Instantiate(card, parent);

                                }
                            }
                            else
                            {
                                OnGoingCards.Add(AvailableCards[chosenCardNumber]);
                                ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                                //   Instantiate(card, parent);
                            }

                        }

                    }
                    else if (words[0] == "Wild")
                    {


                        card.GetComponent<CardsExample>().Color = words[0];
                        card.GetComponent<CardsExample>().number = (int.Parse(words[1]));
                        ws.SendCards(player.GetComponent<PlayerManager>().username, words[0], int.Parse(words[1]));
                        //  Instantiate(card, parent);

                    }

                    Instantiate(card, parent);
                }
                i++;
            }
        }
        if(didItComeFromPlayer == true)
        {
            EndTurn();
        }
    }

    public void DeletePlayerCard(string cardColor,int cardId)
    {
        int i = 0;

        foreach (var player in AvailablePlayers)
        {
            var getPlayer = teams.GetChild(player.GetComponent<PlayerManager>().number);
            if (currentPlayerTurn == i)
            {
                Debug.Log("deleting");
                var card = getPlayer.Find($"CardHands/{cardColor}{cardId}");

                if (card == null)
                {
                    Debug.Log("yeet");
                }
                Destroy(card.gameObject);

            }
            i++;
        }

        i = 0;

    }

    void TypeOfCard(string color, int id)
    {
        OnPlayerFinished();
        Debug.Log($"TypeOfCard: {color}_{id}");
        if(color != "Wild")
        {
            if(id > 9)
            {
                if(id == 10)
                {
                    Skip();
                    
                    EndTurn();
                }else if(id == 11)
                {
                    Plustwo();
                    EndTurn();
                   
                }else if (id == 12)
                {
                    Transform clockText = GameObject.Find("UICanvas").transform.GetChild(1);
                    if (isClockWise == true)
                    {
                        isClockWise = false;
                      
                        clockText.GetComponent<DirectionSpin>().IsClockWise = true ;

                    }
                    else
                    {
                        isClockWise = true;
                        clockText.GetComponent<DirectionSpin>().IsClockWise = false;
                    }
                   
                    EndTurn();
                }

            }
            else
            {
                EndTurn();
            }

        }
        else if(color == "Wild")
        {


        }
        else
        {
            EndTurn();
        }
    }
    public void Skip()
    {
        OnPlayerFinished();
        Debug.Log("Skip");
        var GotSkipped = GameObject.Find("SkipParticles");
        int i = 0;
        if (isClockWise == true)
        {
            Debug.Log("Clock");
            if (currentPlayerTurn < AvailablePlayers.Count)
            {
                currentPlayerTurn++;
                Debug.Log($"Inside Skip 1 succ{currentPlayerTurn} ");
                if (currentPlayerTurn == AvailablePlayers.Count)
                {
                    currentPlayerTurn = 0;
                }
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside Skip 2 succ{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                       // GotSkipped.SetActive(true);
                     //  GotSkipped.GetComponent<ParticleSystem>().Play();
                    }
                    i++;
                }
            }
            else
            {
                currentPlayerTurn = 0;
                Debug.Log($"Inside Skip 1 fail{currentPlayerTurn} ");
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside Skip 2 fail{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                      //  GotSkipped.SetActive(true);
                     //   GotSkipped.GetComponent<ParticleSystem>().Play();

                    }
                    i++;
                }
            }
        }
        else if (isClockWise == false)
        {
            Debug.Log("counter");
            if (currentPlayerTurn > 0)
            {
                currentPlayerTurn--;
                Debug.Log($"Inside Skip 1 suc{currentPlayerTurn} ");
                if (currentPlayerTurn == 0)
                {
                    currentPlayerTurn = AvailablePlayers.Count - 1;
                    Debug.Log(AvailableCards.Count);
                }
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside Skip 2 suc{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                       
                     //   GotSkipped.SetActive(true);
                      // GotSkipped.GetComponent<ParticleSystem>().Play();
                      
                      
                    }
                    i++;
                }
            }
            else
            {
                currentPlayerTurn = AvailablePlayers.Count -1;
                Debug.Log($"Inside Skip Fail{currentPlayerTurn} ");
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside Skip 2 fail{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                      
                       // GotSkipped.SetActive(true);
                        //GotSkipped.GetComponent<ParticleSystem>().Play();
                    
                    }
                    i++;
                }
            }
        }
        OnPlayerFinished();
    }
    public void Plustwo()
    {
        int i = 0;
        Debug.Log($" Plus {currentPlayerTurn} ");
        if (isClockWise == true)
        {
            Debug.Log("Clock");
            if (currentPlayerTurn < AvailablePlayers.Count)
            {
                currentPlayerTurn++;
                Debug.Log($"Inside plus 1 succ{currentPlayerTurn} ");
                if (currentPlayerTurn == AvailablePlayers.Count)
                {
                    currentPlayerTurn = 0;
                }
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside plus 2 succ{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        GenerateCardToPlayer(2,false);
                    }
                    i++;
                }
            }
            else
            {
                currentPlayerTurn = 0;
                Debug.Log($"Inside plus 1 fail{currentPlayerTurn} ");
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside plus 2 fail{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        GenerateCardToPlayer(2,false);
                    }
                    i++;
                }
            }
        }
        else if (isClockWise == false)
        {
            Debug.Log("Counter");
            if (currentPlayerTurn > 0)
            {
                currentPlayerTurn--;
                Debug.Log($"Inside plus 1 succ{currentPlayerTurn} ");
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside Skip 2 succ{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();

                        GenerateCardToPlayer(2,false);
                    }
                    i++;
                }
            }
            else
            {
                currentPlayerTurn = AvailablePlayers.Count - 1;
                Debug.Log($"Inside Skip 1 fail{currentPlayerTurn} ");
                if (currentPlayerTurn == 0)
                {
                    currentPlayerTurn = AvailablePlayers.Count - 1;
                   
                }
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Inside plus 2 fail{currentPlayerTurn} ");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        GenerateCardToPlayer(2,false);

                    }
                    i++;
                }
            }
        }
        OnPlayerFinished();
    }
    void ShowTurnFunc()
    {
        ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
        ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(1).gameObject.SetActive(true);
        ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(1).GetComponent<ParticleSystem>().Play();
    }
    public void EndTurn()
    {
        OnPlayerFinished();
        int i = 0;
        Debug.Log($"currentPlayer : {currentPlayerTurn}");
        if (isClockWise == true)
        {
            if(currentPlayerTurn < AvailablePlayers.Count)
            {
               
                currentPlayerTurn++;
                Debug.Log($"inside  1 currentPlayer : {currentPlayerTurn}");
               
                if (currentPlayerTurn == AvailablePlayers.Count )
                {
                    
                    currentPlayerTurn = 0;

                }
                foreach (var player in AvailablePlayers) {

                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"inside2 currentPlayer : {currentPlayerTurn}");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        ws.NextTurn(player.GetComponent<PlayerManager>().username);
                    }
                    i++;
                }
            }
            else
            {
              
                currentPlayerTurn = 0;
                Debug.Log($"NotEnough {currentPlayerTurn}");
                
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"inside currentPlayer : {currentPlayerTurn}");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        ws.NextTurn(player.GetComponent<PlayerManager>().username);

                    }
                    i++;
                }
            }
        }
        else if (isClockWise == false)
        {

            Debug.Log("CounterClock");
            if(currentPlayerTurn > 0)
            {
                
                currentPlayerTurn--;
                Debug.Log($"DidGoIn {currentPlayerTurn}");
                if (currentPlayerTurn < 0 )
                {
                    currentPlayerTurn = AvailablePlayers.Count-1;
                }
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"DidGoIn 2{currentPlayerTurn}");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        ws.NextTurn(player.GetComponent<PlayerManager>().username);
                    }
                    i++;
                }
            }
            else
            {
                currentPlayerTurn = AvailablePlayers.Count -1;

                Debug.Log($"Didnt go in {currentPlayerTurn}");
                foreach (var player in AvailablePlayers)
                {
                    if (i == currentPlayerTurn)
                    {
                        Debug.Log($"Didnt go in 2{currentPlayerTurn}");
                        ShowTurn = teams.GetChild(i);
                        ShowTurnFunc();
                        ws.NextTurn(player.GetComponent<PlayerManager>().username);
                    }
                    i++;
                }
            }
        }

       
    }

    private void OnPlayerFinished()
    {
        int i = 0;


        foreach (var player in AvailablePlayers)
        {
            if (i == currentPlayerTurn)
            {

                ShowTurn = teams.GetChild(i);

                ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                ShowTurn.GetComponent<PlayerPlatform>().extras.GetChild(1).gameObject.SetActive(false) ;
                i++;

            }
            else
            {
                i++;
            }
        }

        i = 0;
    }

    public void OnPlayerWin(string user)
    {

     

        Transform Winner = GameObject.Find("UICanvas").transform.GetChild(0);
        Winner.gameObject.SetActive(true);
        TextMeshProUGUI WinnerText = GameObject.Find("OnWinner").GetComponent<TextMeshProUGUI>();
        WinnerText.text += user;
        Winner.GetChild(0).transform.gameObject.SetActive(true);
        Winner.GetChild(0).transform.GetComponent<ParticleSystem>().Play();
        Winner.GetChild(1).transform.gameObject.SetActive(true);

        ws.OnGameFinish();
    }
}
