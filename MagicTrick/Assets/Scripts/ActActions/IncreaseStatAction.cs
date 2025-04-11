using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class IncreaseStatAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public double ChangeAmount = 0;

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // return a function thad adds to input
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x => x + (long)ChangeAmount;
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x => x + ChangeAmount;
                break;
            case Stats.Payout:
                container.PayoutActions += x => x + (long)ChangeAmount;
                break;
            case Stats.Liability:
                container.LiabilityActions += x => x + (long)ChangeAmount;
                break;
        }
        return container;
    }
}
