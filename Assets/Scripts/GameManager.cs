using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

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

    [SerializeField]
    private int minDeckSize = 2; // Auto draws 2 cards if you have less

    public List<Player> players = new();
    public int currentPlayerIndex = 0;

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

        foreach (var card in startingDeck.Cards)
        {
            gameDeck.Add(card.Clone());
        }
        for (var i = 0; i < playerCount; i++)
        {
            var player = new Player();
            players.Add(player);
            player.DrawRandomCard(startingHandSize);
            player.CheckScore();
            player.SortHand();
        }
    }

    public void NextPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;
    }

    public void StealCard(Player currentPlayer, Player playerToSteal)
    {
        //randomly steals a card from the playerToSteal from both hand and special hand
        var random = Random.Range(0, 2);
        if (random == 0) // aka steal from normal hand
        {
            if (playerToSteal.hand.Count == 0)
                random = 1; // if the player has no cards in their hand, steal from special hand
        }
    }
}
