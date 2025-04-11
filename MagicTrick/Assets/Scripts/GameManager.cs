using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    public Party currParty;
    public bool canMulligan = true;

    // The followign are references to the trick slot.
    // Heiarchy goes Trick Slot -> Cardslot prefab -> Then card
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
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private HorizontalCardHolder cardHolder;
    [SerializeField]
    private Button mulliganButton;
    private ScoreManager scoreManager;
    private DeckManager deckManager;
    public HistoryManager historyManager;
    private List<GameObject> slots;
    private List<GameObject> propSlots;

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
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Idle:
                break;
            case GameState.PartyStart:
                state = GameState.RoundStart;
                break;
            case GameState.RoundStart:
                // Pre round effects
                // Reset and shuffle deck
                deckManager.InitializeDeck();
                state = GameState.ActStart;
                break;
            case GameState.ActStart:
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
                // Update score to say what it might be 
                // TODO: DELETE THIS
                PreviewAllCards();
                scoreText.text = $"Current score: {scoreManager.Score} + {scoreManager.PreviewScore - scoreManager.Score}";
                break;
            case GameState.ActPlayout:
                // Play each card
                PlayAllCards();
                // Update text
                scoreText.text = $"Current score: {scoreManager.Score}";
                // Go to next 
                state = GameState.ActEnd;
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
                if (scoreManager.Score < currParty.Rounds[currRound].ScoreRequired)
                {
                    // Die
                    state = GameState.GameOver;
                }
                else
                {
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
                state = GameState.RoundStart;
                break;
            case GameState.PartyEnd:
                break;
            case GameState.GameOver:
                break;
        }

        if (mulliganButton.enabled != canMulligan)
        {
            mulliganButton.enabled = canMulligan;
        }

        // debug stuff
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayableCard draw = deckManager.DrawCard();
            cardHolder.AddCardToHand(draw);
        }
    }

    public void LoadParty(Party party)
    {
        currParty = party;
    }

    private void PlayAllCards()
    {
        // Get the action container for each card, combine them, then calculate
        // what the new score would be. Return the new score.
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
        scoreManager.ApplyToScore(combined, this);

        // Now update the timeline
        for (int i = 0; i < historyManager.slotTimelines.Count; i++)
        {
            historyManager.slotTimelines[i].History[0] = currentSlotContainer[i];
            historyManager.slotTimelines[i].AdvanceTime();
        }
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
        scoreManager.ApplyToPreviewScore(combined);
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
        // If you get here, all slots are empty
        return true;
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
        for (int i = 0; i < cardObjs.Count; i++)
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
        state = GameState.ActPlayout;
    }

    public void SetSlotEnable(int slot, bool isEnabled)
    {
        Debug.Log($"Modify slot {slot}, setting enabled to: {isEnabled}");
    }
}
