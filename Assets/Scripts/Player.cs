using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Player
{
    public List<CardDef> hand = new();
    public List<CardDef> handSpecial = new();
    public List<CardDef> bank = new();
    public int score = 0;

    #region draw cards
    public void DrawRandomCard(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            DrawRandomCard();
        }
    }

    public void DrawRandomCard()
    {
        int failSafe = 0;
        while (true)
        {
            failSafe++;
            if (failSafe > 100)
            {
                Debug.LogError("Failed to draw a card");
                break;
            }
            var state = PrivateDraw();
            switch (state)
            {
                case Check.GameOutOfCardsSingleType:
                    continue;
                case Check.GameOutOfCardsTotal:
                    Debug.Log("Game is out of cards");
                    break;
            }

            break;
        }
    }

    private Check PrivateDraw()
    {
        if (GameManager.Main.gameDeck.Count == 0)
            return Check.GameOutOfCardsTotal;

        var card = GameManager.Main.gameDeck[Random.Range(0, GameManager.Main.gameDeck.Count)];
        if (card.amount <= 0)
        {
            // GameManager.Main.gameDeck.Remove(card);
            return Check.GameOutOfCardsSingleType;
        }

        card.amount--;
        if (card.cardDef.Type == CardType.Special)
            handSpecial.Add(card.cardDef);
        else
            hand.Add(card.cardDef);
        return Check.Done;
    }
    #endregion

    public void CheckScore()
    {
        score = 0;
        foreach (var card in bank)
            score += card.CardWorth;
        foreach (var card in hand)
            score += card.CardWorth * 2;
        foreach (var card in handSpecial)
            score += card.CardWorth * 2;
    }

    public void SortHand() // sorts by card type then by card name
    {
        SortDefaultCards();
        SortSpecialCards();
    }

    private void SortDefaultCards() // sorts by common to legendary
        =>
        hand.Sort(
            (a, b) =>
            {
                var typeComparison = a.CardWorth.CompareTo(b.CardWorth);
                return typeComparison;
            }
        );

    private void SortSpecialCards() // Sorts by name
        => handSpecial.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

    private enum Check
    {
        GameOutOfCardsTotal,
        GameOutOfCardsSingleType,
        Done
    }

    public void PlaySpecialCard(CardDef card)
    {
        if (!handSpecial.Contains(card))
            return; // error handing later
        handSpecial.Remove(card);
    }

    #region all Special Cards methods
    public void Withdraw()
    {
        // get from your own bank
    }

    public void StealBank()
    {
        // steal from another player's bank
    }

    public void TripleStealHand()
    {
        // steal 3 cards from another player's hand
        // choosing logic will be implemented later
        GameManager.Main.players[GameManager.Main.currentPlayerIndex + 1].h
    }

    public void Bomb()
    {
        // forces all the chosen player's cards to the bank
    }

    public void SecureCard()
    {
        // ?
    }

    public void Draw4() => DrawRandomCard(4);

    public void Draw2() => DrawRandomCard(2);
    

    public void Denied()
    {
        // blocks a special card
    }
    #endregion
}
