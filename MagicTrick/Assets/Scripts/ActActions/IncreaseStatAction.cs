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
    public long ChangeAmount = 0;

    public override ActionContainer AddAction(ActionContainer container, GameManager manager)
    {
        // return a function thad adds to input
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x => x + ChangeAmount;
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x => x + ChangeAmount;
                break;
            case Stats.Payout:
                container.PayoutActions += x => x + ChangeAmount;
                break;
            case Stats.Liability:
                container.LiabilityActions += x => x + ChangeAmount;
                break;
        }
        return container;
    }
}
