using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StartingDeck : ScriptableObject
{
    [field: SerializeField]
    public List<Card> Cards { get; private set; }
}

[Serializable]
public class Card
{
    public int amount;
    public CardDef cardDef;

    public Card Clone()
    {
        return new Card { amount = amount, cardDef = cardDef };
    }
}

public enum CardType
{
    Default,
    Special
}

public enum SpecialType
{
    Withdraw, // get from your own bank
    StealBank, // steal from another player's bank
    TripleStealHand, // steal 3 cards from another player's hand
    Bomb, // forces all the chosen player's cards to the bank
    SecureCard, // ?
    Minus10, // -10 points
    Minus25, // -25 points
    Draw4, // draw 4 cards
    Draw2, // draw 2 cards
    Denied // blocks a special card
}
