using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    Idle,
    PartyStart,
    RoundStart,
    ActStart,
    Mulligan,
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

    // The followign are references to the trick slot.
    // Heiarchy goes Trick Slot -> Cardslot prefab -> Then card
    [SerializeField]
    private GameObject slot1;
    [SerializeField]
    private GameObject slot2;
    [SerializeField]
    private GameObject slot3;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private HorizontalCardHolder cardHolder;
    private ScoreManager scoreManager;
    private DeckManager deckManager;
    private List<GameObject> slots;

    void Awake()
    {
        slots = new List<GameObject>
        {
            slot1, slot2, slot3
        };
    }

    void Start()
    {
        //state = GameState.Idle;
        scoreManager = GetComponent<ScoreManager>();
        deckManager = GetComponent<DeckManager>();
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Idle:
                break;
            case GameState.PartyStart:
                // Reset and shuffle deck
                deckManager.InitializeDeck();
                state = GameState.RoundStart;
                break;
            case GameState.RoundStart:
                // Pre round effects
                state = GameState.ActStart;
                break;
            case GameState.ActStart:
                // Draw cards
                if (cardHolder.cards.Count < maxHandSize)
                {
                    PlayableCard draw = deckManager.DrawCard();
                    cardHolder.AddCardToHand(draw);
                }
                else
                {
                    state = GameState.Mulligan;
                }
                break;
            case GameState.ActSetup:
                // Pick cards to play. Wait for confirmation
                // Update score to say what it might be 
                // TODO: DELETE THIS
                PreviewAllCards();
                scoreText.text = $"Current score: {scoreManager.Score} + {scoreManager.TemporaryScore - scoreManager.Score}";
                // TODO: DELTE THIS
                if (Input.GetKeyUp(KeyCode.O))
                {
                    state = GameState.ActPlayout;
                }
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
                        state = GameState.RoundStart;
                    }
                }
                break;
            case GameState.Mulligan:
                // Wait until mulligan has finished.
                state = GameState.ActSetup;
                break;
            case GameState.PartyEnd:
                break;
            case GameState.GameOver:
                break;
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
        foreach (var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                currCardObj.CardData.PlayCard(this);
            }
        }
    }

    private void PreviewAllCards()
    {
        // Zero out the temp variables in score manager first 
        scoreManager.ClearToAddVariables();

        // Run preview card in all slots if there is something there
        foreach (var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                currCardObj.CardData.PreviewCard(this);
            }
        }
    }

    private bool DiscardBoard()
    {
        foreach(var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                // Add the card to discards
                deckManager.SendToDiscard(currCardObj.CardData);

                // Delete the game object
                Destroy(currCardObj.transform.parent.gameObject);
                if(cardHolder.cards.Contains(currCardObj))
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
}
