using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyCurrentEffectAction : ActAction
{
    public int MultiplyAmount = 1;

    public override ActionContainer AddAction(ActionContainer container, GameManager manager)
    {
        // To multiply an existing card's effects, we are going to loop
        // over all the stat changes and apply them again.
        Func<int, int> temp = container.CaptivationActions;
        foreach (var @delegate in container.CaptivationActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            for(int i=0;i<MultiplyAmount-1; i++)
            {
                temp += currAction;
            }
        }
        container.CaptivationActions = temp;

        // Sleight of hand
        temp = container.SleightOfHandActions;
        foreach (var @delegate in container.SleightOfHandActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            for (int i = 0; i < MultiplyAmount-1; i++)
            {
                temp += currAction;
            }
        }
        container.SleightOfHandActions = temp;

        // Payout
        temp = container.PayoutActions;
        foreach (var @delegate in container.PayoutActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            for (int i = 0; i < MultiplyAmount-1; i++)
            {
                temp += currAction;
            }
        }
        container.PayoutActions = temp;

        // Liability
        temp = container.LiabilityActions;
        foreach (var @delegate in container.LiabilityActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            for (int i = 0; i < MultiplyAmount-1; i++)
            {
                temp += currAction;
            }
        }
        container.LiabilityActions = temp;
        return container;
    }
}
