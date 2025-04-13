using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    public List<Party> PartyList;
    public int currPartyIndex = 0;
    [SerializeField]
    private GameObject invitationPanel;
    [SerializeField]
    private TMP_Text partyNameText;
    [SerializeField]
    private TMP_Text partyDescriptionText;
    [SerializeField]
    private TMP_Text childNameText;
    [SerializeField]
    private TMP_Text childDecText;
    [SerializeField]
    private TMP_Text childRequiredScoreText;
    [SerializeField]
    private Image childSprite;
    private GameManager gameManager;

    void Start()
    {
        currPartyIndex = 0;
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInviteScreen()
    {
        partyNameText.text = PartyList[currPartyIndex].PartyName;
        partyDescriptionText.text = PartyList[currPartyIndex].PartyDescription;
        invitationPanel.SetActive(true);
    }

    public void UpdateChildInfo(int roundIndex = 0)
    {
        childNameText.text = $"{PartyList[currPartyIndex].Rounds[roundIndex].RoundName} - Act {gameManager.currAct}";
        childDecText.text = PartyList[currPartyIndex].Rounds[roundIndex].RoundEffect;
        childRequiredScoreText.text = $"{PartyList[currPartyIndex].Rounds[roundIndex].ScoreRequired:n0}";
        childSprite.sprite = PartyList[currPartyIndex].Rounds[roundIndex].RoundImage;
    }

    public bool NextParty()
    {
        currPartyIndex++;
        return currPartyIndex < PartyList.Count;
    }

    public bool CheckIfRoundPass(int roundIndex, long score)
    {
        return score >= PartyList[currPartyIndex].Rounds[roundIndex].ScoreRequired;
    }

    public long GetRoundScoreRequired(int roundIndex)
    {
        return PartyList[currPartyIndex].Rounds[roundIndex].ScoreRequired;
    }

    public void HideInviteScreen()
    {
        invitationPanel.SetActive(false);
    }

    public ActionContainer ApplyChildEffect(ActionContainer container, int cardSlot, int roundIndex)
    {
        foreach(var roundAct in PartyList[currPartyIndex].Rounds[roundIndex].Actions)
        {
            container = roundAct.AddAction(container, cardSlot, gameManager);
        }
        return container;
    }
}
