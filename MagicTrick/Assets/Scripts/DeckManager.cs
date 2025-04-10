using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // The deck only holds all the cards possible in the deck.
    public Deck CurrentDeck;

    // Manage what is currently in the deck and discards
    public List<PlayableCard> DeckCards;
    public List<PlayableCard> Discards;

    [SerializeField]
    private TMP_Text DeckText;
    private System.Random rand;

    public event Action OnDeckChanged;

    void Awake()
    {
        rand = new System.Random();
    }

    void Start()
    {
        DeckCards = new List<PlayableCard>();
        Discards = new List<PlayableCard>();
        InitializeDeck();
    }

    void Update()
    {
        DeckText.text = $"{DeckCards.Count}";
    }

    public void InitializeDeck()
    {
        // Clear the decks
        DeckCards.Clear();
        Discards.Clear();

        // Copy the card list from the deck
        DeckCards.AddRange(CurrentDeck.PlayableCardList);
        OnDeckChanged?.Invoke();
        // Shuffle
        ShuffleCurrentDeck();
    }

    public void RefreshDeck()
    {
        // Move all discards back to the deck cards and shuffle
        DeckCards.AddRange(Discards);
        Discards.Clear();
        ShuffleCurrentDeck();
        OnDeckChanged?.Invoke();
    }


    public void RemoveCard(PlayableCard card)
    {
        if (DeckCards.Remove(card))
        {
            Debug.Log($"Successfully removed card: {card.CardName}");
        }
        else
        {
            Debug.LogWarning($"Card not found in deck: {card.CardName}");
        }

        OnDeckChanged?.Invoke();
    }
    public void ShuffleCurrentDeck()
    {
        DeckCards = DeckCards.OrderBy(x => rand.NextDouble()).ToList();
    }

    public PlayableCard DrawCard()
    {
        var toreturn = DeckCards.FirstOrDefault();
        if (toreturn == null)
        {
            return null;
        }
        else
        {
            DeckCards.RemoveAt(0);
            if (DeckCards.Count == 0)
            {
                // Deck refresh
                RefreshDeck();
            }
            OnDeckChanged?.Invoke();
            return toreturn;
        }
    }

    public void SendToDiscard(PlayableCard card)
    {
        Discards.Add(card);
        OnDeckChanged?.Invoke();
    }
}
