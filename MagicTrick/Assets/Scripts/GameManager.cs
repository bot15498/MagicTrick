using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Idle,
    PartyStart,
    RoundStart,
    ActStart,
    ActSetup,
    ActPlayout,
    ActEnd,
    RoundEnd,
    Shopping,
    PartyEnd,
    GameOver,
    YouWin,
}

public enum Slot
{
    Slot1,
    Slot2,
    Slot3,
}

public class GameManager : MonoBehaviour
{
    public int currRound = 1;
    public int currAct = 1;
    public int maxRounds = 3;
    public int maxAct = 3;
    public int maxHandSize = 8;
    public GameState state;
    public bool canMulligan = true;

    // The followign are references to the trick slot.
    // Heiarchy goes Trick Slot -> Cardslot prefab -> Then card
    [SerializeField]
    private HorizontalCardHolder cardHolder;
    [SerializeField]
    private Button mulliganButton;
    // Stuff for playing cards
    private List<ActionContainer> currCardContainers = new List<ActionContainer>();
    private List<PlayableCard> currCardData = new List<PlayableCard>();
    [SerializeField]
    private int currCardContainerIndex = 0;
    [SerializeField]
    private bool isDoingAnimation = false;
    [SerializeField]
    private float elapsedTime = 0f;
    [SerializeField]
    private float maxAnimationElapsedTime = 7f;
    [Header("Trick slots and prop slots")]
    [SerializeField]
    private GameObject slot1;
    [SerializeField]
    private GameObject slot2;
    [SerializeField]
    private GameObject slot3;
    [SerializeField]
    private GameObject propSlot1;
    [SerializeField]
    private GameObject propSlot2;
    [SerializeField]
    private GameObject propSlot3;
    [Header("Stat text fields")]
    [SerializeField]
    private TMP_Text totalScoreText;
    [SerializeField]
    private TMP_Text captivationText;
    [SerializeField]
    private TMP_Text captivationAddText;
    //[SerializeField]
    //private TMP_Text captivationSignText;
    [SerializeField]
    private TMP_Text sleightOfHandText;
    [SerializeField]
    private TMP_Text sleightOfHandAddText;
    //[SerializeField]
    //private TMP_Text sleightOfHandSignText;
    [SerializeField]
    private TMP_Text additionalPayoutText;
    [SerializeField]
    private TMP_Text additionalPayoutAddText;
    [SerializeField]
    private TMP_Text additionalPayoutSignText;
    [SerializeField]
    private TMP_Text liabilityText;
    [SerializeField]
    private TMP_Text liabilityAddText;
    [SerializeField]
    private TMP_Text liabilitySignText;
    [SerializeField]
    private TMP_Text moneyText;
    [SerializeField]
    private TMP_Text moneyText2;
    [Header("Shop stuff")]
    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    private GameObject sideCurtain;
    [SerializeField]
    private float shopTransitionDuration;
    private bool didShopUpdate = false;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private float gameOverTransitionDuration = 0.5f;
    private bool calledYouLose = false;
    public ScoreManager scoreManager;
    public DeckManager deckManager;
    private ShopManager shopManager;
    public HistoryManager historyManager;
    public PropManagerGlobal propManagerGlobal;
    public AnimationManager animationManager;
    private PartyManager partyManager;
    public List<GameObject> slots;
    public List<GameObject> propSlots;
    [SerializeField] float reloadDelay = 0.1f;
    void Awake()
    {
        slots = new List<GameObject>
        {
            slot1, slot2, slot3
        };
        propSlots = new List<GameObject>
        {
            propSlot1, propSlot2, propSlot3
        };
    }

    void Start()
    {
        //state = GameState.Idle;
        scoreManager = GetComponent<ScoreManager>();
        deckManager = GetComponent<DeckManager>();
        historyManager = GetComponent<HistoryManager>();
        propManagerGlobal = GetComponent<PropManagerGlobal>();
        shopManager = GetComponent<ShopManager>();
        partyManager = GetComponent<PartyManager>();
        shopPanel.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Idle:
                break;
            case GameState.PartyStart:
                // Show the party text
                UpdateScoreFields();
                partyManager.ShowInviteScreen();
                partyManager.UpdateChildInfo();
                break;
            case GameState.RoundStart:
                // Pre round effects
                // Reset and shuffle deck
                scoreManager.ResetStats();
                deckManager.RefreshDeck();
                partyManager.UpdateChildInfo(currRound - 1);
                historyManager.ClearSlotTimelines();
                state = GameState.ActStart;
                break;
            case GameState.ActStart:
                partyManager.UpdateChildInfo(currRound - 1);
                // Draw cards
                canMulligan = true;
                ResetCardSelection(canMulligan);
                if (cardHolder.cards.Count < maxHandSize)
                {
                    PlayableCard draw = deckManager.DrawCard();
                    cardHolder.AddCardToHand(draw);
                }
                else
                {
                    state = GameState.ActSetup;
                }
                break;
            case GameState.ActSetup:
                // Pick cards to play. Wait for confirmation
                PreviewAllCards();
                UpdateScoreFields();
                break;
            case GameState.ActPlayout:
                // Play each card
                if (DiscardBoard() && PlayAllCards())
                {
                    UpdateScoreFields();
                    // Go to next 
                    state = GameState.ActEnd;
                }
                // Update text
                UpdateScoreFields();
                break;
            case GameState.ActEnd:
                // Send to discards
                bool allgood = DiscardBoard();
                if (allgood)
                {
                    // Determine what to do next
                    currAct++;
                    if (currAct > maxAct)
                    {
                        currAct = 1;
                        state = GameState.RoundEnd;
                    }
                    else
                    {
                        state = GameState.ActStart;
                    }
                }
                break;
            case GameState.RoundEnd:
                // Check if you die or not
                if (!partyManager.CheckIfRoundPass(currRound - 1, scoreManager.Score))
                {
                    // Die
                    state = GameState.GameOver;
                }
                else if (scoreManager.additionalPayout < scoreManager.liability)
                {
                    // die 
                    state = GameState.GameOver;
                }
                else
                {
                    // Calculate money
                    scoreManager.UpdateMoney(partyManager.GetRoundScoreRequired(currRound - 1));
                    // Send your hand to your discards.
                    SendHandToDiscard();
                    deckManager.RefreshDeck();
                    // Advance to the shop
                    currRound++;
                    if (currRound > maxRounds)
                    {
                        currRound = 1;
                        state = GameState.PartyEnd;
                    }
                    else
                    {
                        state = GameState.Shopping;
                    }
                }
                break;
            case GameState.Shopping:
                UpdateScoreFields();
                // Show the shop
                if (!shopPanel.activeSelf)
                {
                    // REactive shop
                    propManagerGlobal.PropShopManager.SetPropContainerActive(true);
                    propManagerGlobal.PropShopManager.ClearProps();
                    // Tween it in. When it gets halfway down, then update shop 
                    var tween = shopPanel.transform.DOLocalMoveY(0f, shopTransitionDuration);
                    tween.OnUpdate(() =>
                    {
                        if (!didShopUpdate && tween.position > shopTransitionDuration / 2)
                        {
                            // Update the prop inventory in the shop
                            //propManagerGlobal.UpdateShopPropManager();
                            // disable the propslots and inventory onthe table
                            propManagerGlobal.PropTableManager.gameObject.SetActive(false);
                            foreach (var propslot in propSlots)
                            {
                                propslot.SetActive(false);
                            }
                            //shopManager.RefreshPropShop(doForFree: true);
                            didShopUpdate = true;
                        }
                        else if (didShopUpdate && tween.position > shopTransitionDuration / 2)
                        {
                            propManagerGlobal.PropShopManager.ForceUpdatePropVisual();
                        }
                    });
                    tween.OnComplete(() =>
                    {
                        //// Update the prop inventory in the shop
                        propManagerGlobal.UpdateShopPropManager();
                        shopManager.RefreshPropShop(doForFree: true);
                        didShopUpdate = false;
                    });
                    shopPanel.SetActive(true);
                }
                // Pull the curtain down
                break;
            case GameState.PartyEnd:
                // Should this be a party summary?
                if (partyManager.NextParty())
                {
                    // go next
                    state = GameState.PartyStart;
                }
                else
                {
                    // Out of parties
                    state = GameState.YouWin;
                }
                break;
            case GameState.GameOver:
                Debug.Log("YOU LOSE");
                YouLose();
                break;
            case GameState.YouWin:
                Debug.Log("YOU WIN");
                break;
        }

        if (mulliganButton.interactable != canMulligan)
        {
            mulliganButton.interactable = canMulligan;
        }

        // debug stuff
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayableCard draw = deckManager.DrawCard();
            cardHolder.AddCardToHand(draw);
        }
    }

    private bool PlayAllCards()
    {
        elapsedTime += Time.deltaTime;
        // This returns true when all animations played and scores apply
        if (currCardContainerIndex < currCardContainers.Count || isDoingAnimation)
        {
            if (!isDoingAnimation)
            {
                elapsedTime = 0f;
                isDoingAnimation = true;
                // not waiting for anything, start animation
                PlayableCard currCardObj = currCardData[currCardContainerIndex];
                if (currCardObj != null)
                {
                    animationManager.playAnimation(currCardObj.AnimationName);
                }
                else
                {
                    // Nothing to do.
                    isDoingAnimation = false;
                }
                // Play the card
                scoreManager.ApplyToScore(currCardContainers[currCardContainerIndex], this);
                // Now update the timeline
                historyManager.slotTimelines[currCardContainerIndex].History[0] = currCardContainers[currCardContainerIndex];
                historyManager.slotTimelines[currCardContainerIndex].AdvanceTime();
                currCardContainerIndex++;
            }
            else if (elapsedTime >= maxAnimationElapsedTime)
            {
                // took too long
                elapsedTime = 0f;
                isDoingAnimation = false;
            }
            return false;
        }
        else
        {
            // Nothing left to do
            currCardContainerIndex = 0;
            animationManager.playAnimation("Magician_Idle");
            return true;
        }
    }

    private void SetupCardActionsToPlay()
    {
        // Get the action container for each card, combine them, then calculate
        // what the new score would be. Return the new score.
        currCardContainers.Clear();
        currCardData.Clear();
        currCardContainers = CreateCombinedContainer();
        // Combine with anything thats currently in the history
        for (int i = 0; i < historyManager.slotTimelines.Count; i++)
        {
            currCardContainers[i] = currCardContainers[i] + historyManager.slotTimelines[i].History[0];
        }
        // Get the card data if it exists
        foreach(var cardslot in slots)
        {
            Card currCardObj = cardslot.GetComponentInChildren<Card>();
            if(currCardObj != null)
            {
                currCardData.Add(currCardObj.CardData);
            }
            else
            {
                currCardData.Add(null);
            }
        }
        // Clear animation stuff
        elapsedTime = 0f;
        isDoingAnimation = false;
    }

    private void PreviewAllCards()
    {
        // Get the current action containers
        List<ActionContainer> currentSlotContainer = CreateCombinedContainer();
        // Combine with anything thats currently in the history
        for (int i = 0; i < historyManager.slotTimelines.Count; i++)
        {
            currentSlotContainer[i] = currentSlotContainer[i] + historyManager.slotTimelines[i].History[0];
        }
        ActionContainer combined = new ActionContainer();
        foreach (ActionContainer actionContainer in currentSlotContainer)
        {
            combined += actionContainer;
        }
        scoreManager.ApplyToPreviewScore(combined, this);
    }

    private List<ActionContainer> CreateCombinedContainer()
    {
        // Get the action container for each card, combine them, then calculate
        List<ActionContainer> slotchanges = new List<ActionContainer>();
        for (int currCardSlot = 0; currCardSlot < slots.Count; currCardSlot++)
        {
            ActionContainer previewContainer = new ActionContainer();
            Card currCardObj = slots[currCardSlot].GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                previewContainer = currCardObj.CardData.ApplyCard(previewContainer, currCardSlot, this);
            }
            for (int currPropSlot = 0; currPropSlot < propSlots.Count; currPropSlot++)
            {
                Prop currPropObj = propSlots[currPropSlot].GetComponentInChildren<Prop>();
                if (currPropObj != null)
                {
                    previewContainer = currPropObj.PropData.ApplyProp(previewContainer, this, currCardSlot, currPropSlot);
                }
            }
            // Add any child / round effects if they exist
            previewContainer = partyManager.ApplyChildEffect(previewContainer, currCardSlot, currRound - 1);
            slotchanges.Add(previewContainer);
        }
        return slotchanges;
    }

    private bool DiscardBoard()
    {
        foreach (var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                // Add the card to discards
                deckManager.SendToDiscard(currCardObj.CardData);

                // Delete the game object
                Destroy(currCardObj.transform.parent.gameObject);
                if (cardHolder.cards.Contains(currCardObj))
                {
                    cardHolder.cards.Remove(currCardObj);
                }
                // Exit early
                return false;
            }
        }
        // Now send all props back to your inventory
        foreach (var propslot in propSlots)
        {
            Prop currPropObj = propslot.GetComponentInChildren<Prop>();
            if (currPropObj != null)
            {
                propManagerGlobal.PropTableManager.AssignPropToEmptySlot(currPropObj);
                // exit early
                return false;
            }
        }
        // If you get here, all slots are empty
        return true;
    }

    private void SendHandToDiscard()
    {
        foreach (var card in cardHolder.cards)
        {
            deckManager.SendToDiscard(card.CardData);
            Destroy(card.transform.parent.gameObject);
        }
        cardHolder.cards.Clear();
    }

    public void Mulligan()
    {
        if (!canMulligan)
        {
            // nope
            return;
        }
        List<GameObject> cardObjs = new List<GameObject>();
        // Get all selected cards in hand
        foreach (var card in cardHolder.cards)
        {
            if (card.selected)
            {
                cardObjs.Add(card.transform.parent.gameObject);
            }
        }
        // Get all selected cards played in slots
        foreach (var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null && currCardObj.selected)
            {
                cardObjs.Add(currCardObj.transform.parent.gameObject);
            }
        }

        // Check if you selected anything. If not, don't do anything.
        if (cardObjs.Count == 0)
        {
            return;
        }

        // Go one by one and delete / draw a card
        int startCardCounToKill = cardObjs.Count;
        for (int i = 0; i < startCardCounToKill; i++)
        {
            GameObject cardToKill = cardObjs.FirstOrDefault();
            if (cardToKill != null)
            {
                Card cardCardTokill = cardToKill.GetComponentInChildren<Card>();
                cardObjs.RemoveAt(0);
                deckManager.SendToDiscard(cardCardTokill.CardData);
                Destroy(cardToKill);
                if (cardHolder.cards.Contains(cardCardTokill))
                {
                    cardHolder.cards.Remove(cardCardTokill);
                }

                PlayableCard draw = deckManager.DrawCard();
                cardHolder.AddCardToHand(draw);
            }
        }

        // Prevent further mulligan
        canMulligan = false;
        ResetCardSelection(false);
    }

    public void ResetCardSelection(bool canBeSelected)
    {
        foreach (var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                currCardObj.SetSelected(false);
                currCardObj.canBeSelected = canBeSelected;
            }
        }
        foreach (var card in cardHolder.cards)
        {
            card.SetSelected(false);
            card.canBeSelected = canBeSelected;
        }
    }

    public void GoToPlayCards()
    {
        // Set up the cards you are going to play
        SetupCardActionsToPlay();
        state = GameState.ActPlayout;
    }

    public void LeaveShop()
    {
        // Start tweening out
        didShopUpdate = false;
        var tween = shopPanel.transform.DOLocalMoveY(1166f, shopTransitionDuration);
        tween.OnUpdate(() =>
        {
            if (!didShopUpdate && tween.position >= shopTransitionDuration / 2)
            {
                // Update the table props
                propManagerGlobal.UpdateTablePropManager();
                propManagerGlobal.PropTableManager.gameObject.SetActive(true);
                foreach (var propslot in propSlots)
                {
                    propslot.SetActive(true);
                }
                // Hide all the shop prop slots
                propManagerGlobal.PropShopManager.SetPropContainerActive(false);
                state = GameState.RoundStart;
                didShopUpdate = true;
            }
        });
        tween.OnComplete(() =>
        {
            didShopUpdate = false;
            shopPanel.SetActive(false);
        });
    }

    public void SetSlotEnable(int slot, bool isEnabled)
    {
        Debug.Log($"Modify slot {slot}, setting enabled to: {isEnabled}");
    }

    public void UpdateScoreFields()
    {
        totalScoreText.text = $"{scoreManager.Score:n0}";
        moneyText.text = $"{scoreManager.money:n0}";
        moneyText2.text = $"{scoreManager.money:n0}";

        // Captivation
        long capToAdd = scoreManager.previewCaptivation - scoreManager.captivation;
        //captivationText.text = $"{scoreManager.captivation:n0}";
        captivationText.GetComponent<IncrementNumberText>().TargetValue = scoreManager.captivation;
        captivationAddText.text = capToAdd < 0 ? "-" : "+" + $"{Math.Abs(capToAdd):n0}";
        //captivationSignText.text = capToAdd < 0 ? "-" : "+";

        // Sleight of hand
        double sohToadd = scoreManager.previewSleightOfHand - scoreManager.sleightOfHand;
        sleightOfHandText.text = scoreManager.sleightOfHand.ToString("#,##0.##");
        sleightOfHandAddText.text = sohToadd < 0 ? "-" : "+" + Math.Abs(sohToadd).ToString("#,##0.##");
        //sleightOfHandSignText.text = sohToadd < 0 ? "-" : "+";

        // liability
        double liabilityToAdd = scoreManager.previewLiability - scoreManager.liability;
        liabilityText.text = $"${scoreManager.liability:n0}";
        liabilityAddText.text = $"{Math.Abs(liabilityToAdd):n0}";
        liabilitySignText.text = liabilityToAdd < 0 ? "-" : "+";

        // payout
        double payoutToAdd = scoreManager.previewAdditionalPayout - scoreManager.additionalPayout;
        additionalPayoutText.text = $"${scoreManager.additionalPayout:n0}";
        additionalPayoutAddText.text = $"{Math.Abs(payoutToAdd):n0}";
        additionalPayoutSignText.text = payoutToAdd < 0 ? "-" : "+";
    }

    public void SetIsDoingAnimation(bool val)
    {
        isDoingAnimation = val;
    }

    public void StartParty()
    {
        partyManager.HideInviteScreen();
        state = GameState.RoundStart;
    }

    public void YouLose()
    {
        if (!calledYouLose)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.DOMoveY(0f, gameOverTransitionDuration);
            calledYouLose = true;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator UpdateNumericalText(TMP_Text textui, long valToChangeTo, string pref)
    {
        int numstep = 3;
        string currVal = textui.text;
        long parsedCurrVal = long.Parse(currVal.Substring(pref.Length));
        if(valToChangeTo != parsedCurrVal)
        {
            while(parsedCurrVal != valToChangeTo)
            {
                if (valToChangeTo > parsedCurrVal)
                {
                    // need to increase
                    parsedCurrVal += numstep;
                    parsedCurrVal = parsedCurrVal > valToChangeTo ? valToChangeTo : parsedCurrVal;
                    textui.text = $"{pref}{parsedCurrVal:n0}";
                }
                else
                {
                    // Need to decrease
                    parsedCurrVal -= numstep;
                    parsedCurrVal = parsedCurrVal < valToChangeTo ? valToChangeTo : parsedCurrVal;
                    textui.text = $"{pref}{parsedCurrVal:n0}";
                }
                yield return null;
            }
        }

        // Set it equal one last time
        textui.text = $"{valToChangeTo:n0}";
    }
}
