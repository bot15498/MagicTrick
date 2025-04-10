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
        Func<long, long> temp1 = container.CaptivationActions;
        container.CaptivationActions = x => x;
        foreach (var @delegate in temp1.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            for(int i=0;i<MultiplyAmount; i++)
            {
                container.CaptivationActions += currAction;
            }
        }

        // Sleight of hand
        Func<long, long> temp2 = container.SleightOfHandActions;
        container.SleightOfHandActions = x => x;
        foreach (var @delegate in temp2.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            for (int i = 0; i < MultiplyAmount-1; i++)
            {
                container.SleightOfHandActions += currAction;
            }
        }

        // Payout
        Func<long, long> temp3 = container.PayoutActions;
        container.PayoutActions = x => x;
        foreach (var @delegate in temp3.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            for (int i = 0; i < MultiplyAmount-1; i++)
            {
                container.PayoutActions += currAction;
            }
        }

        // Liability
        Func<long, long> temp4 = container.LiabilityActions;
        container.LiabilityActions = x => x;
        foreach (var @delegate in temp4.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            for (int i = 0; i < MultiplyAmount-1; i++)
            {
                container.LiabilityActions += currAction;
            }
        }
        return container;
    }
}
