using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MultiplyStatAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public double MultiplyAmount = 1.0;

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // return a function thad adds to input
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x => (long)(x * MultiplyAmount);
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x => (long)(x * MultiplyAmount);
                break;
            case Stats.Payout:
                container.PayoutActions += x => (long)(x * MultiplyAmount);
                break;
            case Stats.Liability:
                container.LiabilityActions += x => (long)(x * MultiplyAmount);
                break;
        }
        return container;
    }
}
