using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatOnTurnAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public double ChangeAmount = 0;
    public int TargetAct = 1;

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // return a function thad adds to input
        double amountToChange = manager.currAct == TargetAct ? ChangeAmount : 0;
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x => x + (long)amountToChange;
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x => x + amountToChange;
                break;
            case Stats.Payout:
                container.PayoutActions += x => x + (long)amountToChange;
                break;
            case Stats.Liability:
                container.LiabilityActions += x => x + (long)amountToChange;
                break;
        }
        return container;
    }
}
