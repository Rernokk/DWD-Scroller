using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardModel
{
    [SerializeField]
    private Suit cardSuit = Suit.HEARTS;

    [SerializeField]
    private int cardValue = 0;

    public Suit CardSuit
    {
        get
        {
            return cardSuit;
        }
    }

    public int CardValue
    {
        get
        {
            // Handling for an Ace since Ace makes more sense to represent with "one" but is the highest scoring card.
            if (cardValue == 1)
            {
                return 14;
            }
            return cardValue;
        }
    }
}
