using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatConditionalCardAction : ActAction
{
    public CardType CardTypeToLookFor = CardType.Showmanship;
    public Stats StatToChange = Stats.Captivation;
    public double ChangeAmount = 0;

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {

        // return a function thad adds to input
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x =>
                {
                    Card currCardObj = manager.slots[slot].GetComponentInChildren<Card>();
                    if(currCardObj != null)
                    {
                        return currCardObj.CardData.Type == CardTypeToLookFor ? x + (long)ChangeAmount : x;
                    }
                    else
                    {
                        return x;
                    }
                };
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x =>
                {
                    Card currCardObj = manager.slots[slot].GetComponentInChildren<Card>();
                    if (currCardObj != null)
                    {
                        return currCardObj.CardData.Type == CardTypeToLookFor ? x + ChangeAmount : x;
                    }
                    else
                    {
                        return x;
                    }
                };
                break;
            case Stats.Payout:
                container.PayoutActions += x =>
                {
                    Card currCardObj = manager.slots[slot].GetComponentInChildren<Card>();
                    if (currCardObj != null)
                    {
                        return currCardObj.CardData.Type == CardTypeToLookFor ? x + (long)ChangeAmount : x;
                    }
                    else
                    {
                        return x;
                    }
                };
                break;
            case Stats.Liability:
                container.LiabilityActions += x =>
                {
                    Card currCardObj = manager.slots[slot].GetComponentInChildren<Card>();
                    if (currCardObj != null)
                    {
                        return currCardObj.CardData.Type == CardTypeToLookFor ? x + (long)ChangeAmount : x;
                    }
                    else
                    {
                        return x;
                    }
                };
                break;
        }
        return container;
    }
}
