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
    private ScoreManager scoreManager;

    void Start()
    {
        state = GameState.Idle;
        scoreManager = GetComponent<ScoreManager>();
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
                state = GameState.ActStart;
                break;
            case GameState.ActStart:
                // Draw cards
                state = GameState.Mulligan;
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
                currAct++;
                if (currAct > maxAct)
                {
                    state = GameState.RoundEnd;
                }
                else
                {
                    state = GameState.ActStart;
                }
                break;
            case GameState.RoundEnd:
                // Check if you die or not
                if(scoreManager.Score < currParty.Rounds[currRound].ScoreRequired)
                {
                    // Die
                    state = GameState.GameOver;
                }
                else
                {
                    currRound++;
                    if (currRound > maxRounds)
                    {
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
    }

    public void LoadParty(Party party)
    {
        currParty = party;
    }

    private void PlayAllCards()
    {
        List<GameObject> slots = new List<GameObject>
        {
            slot1, slot2, slot3
        };
        foreach(var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if(currCardObj != null)
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
        List<GameObject> slots = new List<GameObject>
        {
            slot1, slot2, slot3
        };
        foreach (var slot in slots)
        {
            Card currCardObj = slot.GetComponentInChildren<Card>();
            if (currCardObj != null)
            {
                currCardObj.CardData.PreviewCard(this);
            }
        }
    }
}
