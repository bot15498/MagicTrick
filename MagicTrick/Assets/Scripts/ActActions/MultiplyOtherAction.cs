using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyOtherAction : ActAction
{
    public int MultiplyAmount = 1;
    // This cannot be 0 !!!!!!!!!!!
    public int RelativeTimestep = 1;
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        container.NonStatActions += (GameManager x) =>
        {
            // Add an action in the future to double the card in this slot
            // To multiply a cards effect, you just run the function multiple times. 
            ActionContainer future = new ActionContainer();
            future.NonStatActions += (GameManager x) =>
            {
                // Only double the stats for a card to not cause issues.
                Func<long, long> temp1 = future.CaptivationActions;
                future.CaptivationActions = x => x;
                foreach (var @delegate in temp1.GetInvocationList())
                {
                    var currAction = (Func<long, long>)@delegate;
                    for (int i = 0; i < MultiplyAmount; i++)
                    {
                        future.CaptivationActions += currAction;
                    }
                }

                // Sleight of hand
                Func<double, double> temp2 = future.SleightOfHandActions;
                future.SleightOfHandActions = x => x;
                foreach (var @delegate in temp2.GetInvocationList())
                {
                    var currAction = (Func<double, double>)@delegate;
                    for (int i = 0; i < MultiplyAmount; i++)
                    {
                        future.SleightOfHandActions += currAction;
                    }
                }

                // Payout
                Func<long, long> temp3 = future.PayoutActions;
                future.PayoutActions = x => x;
                foreach (var @delegate in temp3.GetInvocationList())
                {
                    var currAction = (Func<long, long>)@delegate;
                    for (int i = 0; i < MultiplyAmount; i++)
                    {
                        future.PayoutActions += currAction;
                    }
                }

                // Liability
                Func<long, long> temp4 = future.LiabilityActions;
                future.LiabilityActions = x => x;
                foreach (var @delegate in temp4.GetInvocationList())
                {
                    var currAction = (Func<long, long>)@delegate;
                    for (int i = 0; i < MultiplyAmount; i++)
                    {
                        future.LiabilityActions += currAction;
                    }
                }
                return 0;
            };
            x.historyManager.slotTimelines[slot].AddToTimeline(future, RelativeTimestep);

            return 0;
        };

        return container;
    }
}
