using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatOnSlotAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public double ChangeAmount = 0;
    public int TargetSlot = 0;

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Check to see if slot matches
        double amountToChange = TargetSlot == slot ? ChangeAmount : 0;
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
