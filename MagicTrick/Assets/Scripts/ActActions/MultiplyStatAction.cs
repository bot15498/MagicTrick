using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MultiplyStatAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public float MultiplyAmount = 1f;

    public override ActionContainer AddAction(ActionContainer container, GameManager manager)
    {
        // return a function thad adds to input
        switch (StatToChange)
        {
            case Stats.Captivation:
                container.CaptivationActions += x => Mathf.FloorToInt(x * MultiplyAmount);
                break;
            case Stats.SleightOfHand:
                container.SleightOfHandActions += x => Mathf.FloorToInt(x * MultiplyAmount);
                break;
            case Stats.Payout:
                container.PayoutActions += x => Mathf.FloorToInt(x * MultiplyAmount);
                break;
            case Stats.Liability:
                container.LiabilityActions += x => Mathf.FloorToInt(x * MultiplyAmount);
                break;
        }
        return container;
    }
}
