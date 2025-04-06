using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // This is the list of playable cards in this deck. 
    public List<PlayableCard> PlayableCardList;
    public List<Card> Cards;

    void Start()
    {
        // Instantiate a list of card objects based on the card list
        foreach (var playableCard in PlayableCardList)
        {
            //GameObject obj = Instantiate
        }
    }

    void Update()
    {
        
    }
}
