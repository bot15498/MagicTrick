using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // The deck only holds all the cards possible in the deck.
    public Deck CurrentDeck;

    // Manage what is currently in the deck and discards
    public List<PlayableCard> DeckCards;
    public List<PlayableCard> Discards;

    void Start()
    {
        DeckCards = new List<PlayableCard>();
        Discards = new List<PlayableCard>();
        InitializeDeck();
    }

    void Update()
    {
        
    }

    public void InitializeDeck()
    {
        // Clear the decks
        DeckCards.Clear();
        Discards.Clear();

        // Copy the card list from the deck
        DeckCards.AddRange(CurrentDeck.PlayableCardList);

        // Shuffle
        ShuffleCurrentDeck();
    }

    public void RefreshDeck()
    {
        // Move all discards back to the deck cards and shuffle
        DeckCards.AddRange(Discards);
        Discards.Clear();
        ShuffleCurrentDeck();
    }

    public void ShuffleCurrentDeck()
    {
        DeckCards = DeckCards.OrderBy(x => Random.value).ToList();
    }

    public PlayableCard DrawCard()
    {
        if(DeckCards.Count == 0)
        {
            // Deck refresh
            RefreshDeck();
        }

        var toreturn = DeckCards.FirstOrDefault();
        if (toreturn == null)
        {
            return null;
        }
        else
        {
            DeckCards.RemoveAt(0);
            return toreturn;
        }
    }

    public void SendToDiscard(PlayableCard card)
    {
        Discards.Add(card);
    }
}
