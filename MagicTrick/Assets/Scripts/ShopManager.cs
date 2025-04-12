using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public long deleteCardCost = 2;

    [SerializeField]
    private GameObject CardSelectionPanel;
    [SerializeField]
    private GameObject CardRemovePanel;
    [SerializeField]
    private Button CardRemoveButton;
    [SerializeField]
    private List<GameObject> CardSlots = new List<GameObject>();
    [SerializeField]
    private GameObject shopCardPrefab;
    [SerializeField]
    private List<ShopCard> DrawnCards = new List<ShopCard>();
    private ItemManager itemManager;
    private ScoreManager scoreManager;
    private DeckManager deckManager;

    void Start()
    {
        itemManager = GetComponent<ItemManager>();
        scoreManager = GetComponent<ScoreManager>();
        deckManager = GetComponent<DeckManager>();
        CardSelectionPanel.SetActive(false);
        CardRemovePanel.SetActive(false);
    }

    void Update()
    {
        if (scoreManager.money < deleteCardCost && CardRemoveButton.enabled)
        {
            CardRemoveButton.enabled = false;
        }
        else if (scoreManager.money >= deleteCardCost && !CardRemoveButton.enabled)
        {
            CardRemoveButton.enabled = true;
        }
    }

    public void BuyPack(CardType type, long cost)
    {
        // deduct money
        scoreManager.money -= cost;

        // Show card selection
        CardSelectionPanel.SetActive(true);

        // Add to temp list of cards 
        for (int i = 0; i < CardSlots.Count; i++)
        {
            PlayableCard cardData = itemManager.GetRandomCard(type);
            GameObject slotObj = Instantiate(shopCardPrefab, CardSlots[i].transform);
            ShopCard card = slotObj.GetComponentInChildren<ShopCard>(true);

            card.CardData = cardData;
            card.name = $"{i}";
            card.UpdateVisual();
            DrawnCards.Add(card);
        }
    }

    public void CloseCardSelection()
    {
        // Add selected cards to your deck (order doesn't  matter)
        deckManager.DeckCards.AddRange(DrawnCards.Where(x => x.selected).Select(x => x.CardData));

        // Close the card selection
        CardSelectionPanel.SetActive(false);

        // Delete the cshop card prefab
        for (int i = 0; i < DrawnCards.Count; i++)
        {
            Destroy(DrawnCards[i].gameObject);
        }

        // Clear the card list
        DrawnCards.Clear();
    }

    public void ShowCardRemovePanel()
    {
        if (scoreManager.money >= deleteCardCost)
        {
            CardRemovePanel.SetActive(true);
        }
    }

    public void CloseCardRemovePanel()
    {
        CardRemovePanel.SetActive(false);
    }
}
