using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("StartingDeck")]
    public StartingDeck startingDeck;
    public List<Card> gameDeck = new();
    public static GameManager Main;

    [Header("Game Settings")]
    [SerializeField]
    private int playerCount = 2;

    [SerializeField]
    private int startingHandSize = 5;

    public int minDeckSize = 2; // Auto draws 2 cards if you have less

    public List<Player> players = new();
    public int playerIndex;

    public void Awake()
    {
        if (Main == null)
            Main = this;
        else
            Destroy(this);
        if (startingDeck != null)
            return;
        Debug.LogError("StartingDeck is not selected in the GameManager");
        enabled = false;
    }

    public void Start()
    {
        var tempInt = startingDeck.Cards.Sum(card => card.amount);
        Debug.Log(tempInt);

        foreach (var card in startingDeck.Cards) // I hate how scriptable objects work
            gameDeck.Add(card.Clone());

        for (var i = 0; i < playerCount; i++)
        {
            var player = new Player();
            players.Add(player);
            player.DrawRandomCard(startingHandSize);
            player.CheckScore();
            player.SortHand();
        }
    }

    private void Update()
    { // testing functions
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextTurn();
            print("Next player selected");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StealCard(players[playerIndex], players[GetNextPlayerIndex()]);
            NextTurn();
            print("Steal action");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            players[playerIndex].DrawRandomCard();
            NextTurn();
            print("Draw random card");
        }
    }

    public void NextTurn()
    {
        playerIndex = GetNextPlayerIndex();
        foreach (var player in players)
            player.NextTurnCleanup();
    }

    private int GetNextPlayerIndex()
    {
        var nextPlayerIndex = playerIndex + 1;
        if (nextPlayerIndex >= players.Count)
            nextPlayerIndex = 0;
        return nextPlayerIndex;
    }

    #region Steal action
    public void StealCard(Player currentPlayer, Player playerToSteal)
    {
        var stealNormal = playerToSteal.hand.Count != 0;
        var stealSpecial = playerToSteal.handSpecial.Count != 0;
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (!stealNormal && !stealSpecial)
            return;
        if (stealNormal && stealSpecial)
        {
            var random = Random.Range(0, 100);
            if (random <= 50)
                StealNormal(currentPlayer, playerToSteal);
            else
                StealSpecial(currentPlayer, playerToSteal);
        }
        else if (stealNormal)
            StealNormal(currentPlayer, playerToSteal);
        else
            StealSpecial(currentPlayer, playerToSteal);
        currentPlayer.CheckScore();
        playerToSteal.CheckScore();
        // add something to check that the players turn is over
    }

    private static void StealNormal(Player currPlayer, Player stealPlayer)
    {
        var cardToSteal = stealPlayer.hand[Random.Range(0, stealPlayer.hand.Count)];
        currPlayer.bank.Add(cardToSteal);
        stealPlayer.hand.Remove(cardToSteal);
    }

    private static void StealSpecial(Player currPlayer, Player stealPlayer)
    {
        var cardToSteal = stealPlayer.handSpecial[Random.Range(0, stealPlayer.handSpecial.Count)];
        currPlayer.bank.Add(cardToSteal);
        stealPlayer.handSpecial.Remove(cardToSteal);
    }
    #endregion
}
