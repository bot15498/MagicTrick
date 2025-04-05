using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDropZone : MonoBehaviour
{
    public Transform handTransform;
    public Card CurrentCard { get; private set; }
    public GameObject cardObjectInSlot;


    public void AssignCard(Card newCard)
    {
        if (CurrentCard == newCard) return;

        // Clear previous zone if card had one
        if (newCard.currentDropZone != null && newCard.currentDropZone != this)
        {
            newCard.currentDropZone.ClearSlot(newCard);
        }

        // Remove existing card in this zone
        if (CurrentCard != null && CurrentCard != newCard)
        {
            CurrentCard.ReturnToHand();
        }

        // Reparent the card’s SLOT
        Transform slot = newCard.transform.parent;
        slot.SetParent(transform, false);
        cardObjectInSlot = slot.gameObject; 
        slot.localPosition = Vector3.zero;

        CurrentCard = newCard;
        newCard.currentDropZone = this;

        // Remove from hand if it was there
        HorizontalCardHolder hand = handTransform.GetComponent<HorizontalCardHolder>();
        if (hand != null)
        {
            hand.cards.Remove(newCard);
        }
    }

    public void ClearSlot(Card card)
    {
        if (CurrentCard == card)
        {
            CurrentCard = null;
            cardObjectInSlot = null; 
            card.currentDropZone = null;
        }
    }

    public void ReturnCardToHand(Card card)
    {
        if (card == null) return;

        Transform slot = card.transform.parent;
        slot.SetParent(handTransform, false);
        slot.localPosition = Vector3.zero;

        HorizontalCardHolder hand = handTransform.GetComponent<HorizontalCardHolder>();
        if (hand != null && !hand.cards.Contains(card))
        {
            hand.cards.Add(card);
        }

        if (CurrentCard == card)
        {
            CurrentCard = null;
        }

        card.currentDropZone = null;
    }
}
