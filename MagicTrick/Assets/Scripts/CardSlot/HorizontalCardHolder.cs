using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int cardsToSpawn = 7;
    public List<Card> cards;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;
    int cardCount = 0;
    public GameObject test;

    bool IsCardInHand(Card card)
    {
        return card != null &&
               card.transform.parent != null &&
               card.transform.parent.parent == this.transform;
    }

    void Start()
    {

        rect = GetComponent<RectTransform>();
        cards = new List<Card>();
        //cards = GetComponentsInChildren<Card>(true).Where(card => card.transform.parent != null && card.transform.parent.parent == this.transform).ToList();

        //StartCoroutine(Frame());

        //IEnumerator Frame()
        //{
        //    yield return new WaitForSecondsRealtime(.1f);
        //    for (int i = 0; i < cards.Count; i++)
        //    {
        //        if (cards[i].cardVisual != null)
        //            cards[i].cardVisual.UpdateIndex(transform.childCount);
        //    }
        //}
    }

    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }


    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0,selectedCard.selectionOffset,0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    void Update()
    {
        if (selectedCard == null || isCrossing)
            return;

        // Skip swap logic if card is not in the hand
        if (!IsCardInHand(selectedCard))
            return;

        for (int i = 0; i < cards.Count; i++)
        {
            // Ensure both cards are in the hand before swapping
            if (!IsCardInHand(cards[i]))
                continue;

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }

    public void AddCardToHand(PlayableCard cardData)
    {
        if (cardData == null)
        {
            Debug.Log("Trying to draw more cards than you have!!");
            // Probably force a game loss ( but this should never happen) 
            return;
        }
        GameObject slotObj = Instantiate(slotPrefab, transform);
        Card card = slotObj.GetComponentInChildren<Card>(true);

        // Add card data
        card.CardData = cardData;
        card.UpdateVisual();

        // Add callbacks
        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
        card.name = cardCount.ToString();

        cardCount++;
        cards.Add(card);
    }

}
