using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckListRemoveUI : MonoBehaviour
{
   public Transform contentHolder;
    public GameObject cardListItemPrefab;

    private DeckManager deckManager;
    private ScoreManager scoreManager;

    void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        scoreManager = deckManager.GetComponent<ScoreManager>();
        deckManager.OnDeckChanged += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        if (deckManager != null)
            deckManager.OnDeckChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        foreach (Transform child in contentHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in deckManager.DeckCards)
        {
            GameObject item = Instantiate(cardListItemPrefab, contentHolder);
            item.GetComponent<CardListItemRemoveUI>().Setup(card);
        }
    }
}
