using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatBasedOnDeckAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public double SleightOfHandScaleFactor = 0.05;
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Same as increase stat, but change amount is based on remaining cards in deck.
        // for sleight of hand, apply multiplier.
        long changeAmount = manager.deckManager.DeckCards.Count;
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x => x + changeAmount;
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x => x + (changeAmount * SleightOfHandScaleFactor);
                break;
            case Stats.Payout:
                container.PayoutActions += x => x + changeAmount;
                break;
            case Stats.Liability:
                container.LiabilityActions += x => x + changeAmount;
                break;
        }

        return container;
    }
}
