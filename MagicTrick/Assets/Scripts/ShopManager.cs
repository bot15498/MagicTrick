using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private long deleteCardCost = 2;
    private long refreshPropCost = 2;
    private long propCost = 5;

    [SerializeField]
    private GameObject CardSelectionPanel;
    [SerializeField]
    private GameObject CardRemovePanel;
    [SerializeField]
    private Button CardRemoveButton;
    [Header("UI stuff")]
    public GameObject shopPanel;
    public GameObject uiCurtain;
    [Header("card stuff")]
    [SerializeField]
    private List<GameObject> CardSlots = new List<GameObject>();
    [SerializeField]
    private GameObject shopCardPrefab;
    [SerializeField]
    private List<ShopCard> DrawnCards = new List<ShopCard>();
    [Header("props stuff")]
    [SerializeField]
    private GameObject shopPropPrefab;
    [SerializeField]
    private List<GameObject> PropSlots = new List<GameObject>();
    [SerializeField]
    private List<Button> propBuyButtons = new List<Button>();
    private List<PlayableProp> currPropsInStore = new List<PlayableProp>();
    [SerializeField]
    private Button refreshButton;
    private ItemManager itemManager;
    private ScoreManager scoreManager;
    private DeckManager deckManager;
    private PropManagerGlobal propManagerGlobal;

    void Start()
    {
        itemManager = GetComponent<ItemManager>();
        scoreManager = GetComponent<ScoreManager>();
        deckManager = GetComponent<DeckManager>();
        propManagerGlobal = GetComponent<PropManagerGlobal>();
        CardSelectionPanel.SetActive(false);
        CardRemovePanel.SetActive(false);
    }

    void Update()
    {
        // Disable the delete card button if not enough money
        if (scoreManager.money < deleteCardCost && CardRemoveButton.interactable)
        {
            CardRemoveButton.interactable = false;
        }
        else if (scoreManager.money >= deleteCardCost && !CardRemoveButton.interactable)
        {
            CardRemoveButton.interactable = true;
        }

        // Disable the refresh button if not enough money
        if (scoreManager.money < refreshPropCost && refreshButton.interactable)
        {
            refreshButton.interactable = false;
        }
        else if (scoreManager.money >= refreshPropCost && !refreshButton.interactable)
        {
            refreshButton.interactable = true;
        }

        // Disable the buy prop button if not enough money
        for (int i = 0; i < propBuyButtons.Count; i++)
        {
            var butt = propBuyButtons[i];
            if (propManagerGlobal.PropShopManager.IsFull() && butt.interactable)
            {
                butt.interactable = false;
            }
            else
            {
                // If something is in the slot, then control prop button enable / disable
                ShopProp prop = PropSlots[i].GetComponentInChildren<ShopProp>();
                if (prop != null)
                {
                    if (scoreManager.money < propCost && butt.interactable)
                    {
                        butt.interactable = false;
                    }
                    else if (scoreManager.money >= propCost && !butt.interactable)
                    {
                        butt.interactable = true;
                    }
                }
                else if (prop == null && butt.interactable)
                {
                    butt.interactable = false;
                }
            }
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

    public void RefreshPropShop(bool doForFree = false)
    {
        if (!doForFree)
        {
            scoreManager.money -= refreshPropCost;
        }
        currPropsInStore.Clear();
        for (int i = 0; i < PropSlots.Count; i++)
        {
            propBuyButtons[i].interactable = true;

            // Delete the prop there.
            ShopProp child = PropSlots[i].GetComponentInChildren<ShopProp>();
            if (child != null)
            {
                Destroy(child.gameObject);
            }

            // Roll a new prop
            // Combine with current prop list to prevent repeats
            var onscreenPropList = propManagerGlobal.PropShopManager.GetPropList();
            onscreenPropList.AddRange(currPropsInStore);
            PlayableProp newprop = itemManager.GetRandomProp(onscreenPropList);

            // Instantiate a new ShopProp
            GameObject slotObj = Instantiate(shopPropPrefab, PropSlots[i].transform);
            ShopProp prop = slotObj.GetComponentInChildren<ShopProp>(true);
            prop.PropData = newprop;
            prop.name = $"{i}";
            prop.UpdateVisual();
            currPropsInStore.Add(newprop);
        }
    }

    public void BuyProp(int index)
    {
        ShopProp child = PropSlots[index].GetComponentInChildren<ShopProp>();
        if (child == null)
        {
            return;
        }

        scoreManager.money -= propCost;

        // Only add to the shop prop manager
        propManagerGlobal.PropShopManager.AssignPropToEmptySlot(child.PropData);

        // Delete the prop there.
        Destroy(child.gameObject);

        // Disable the buy button
        propBuyButtons[index].interactable = false;
    }
}
