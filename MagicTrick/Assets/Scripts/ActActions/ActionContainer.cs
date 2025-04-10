using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContainer
{
    // This is a holder for all the actions you are going to do to the stats this timestep
    public Func<int, int> CaptivationActions { get; set; } = x => x;
    public Func<int, int> SleightOfHandActions { get; set; } = x => x;
    public Func<int, int> PayoutActions { get; set; } = x => x;
    public Func<int, int> LiabilityActions { get; set; } = x => x;

    public int ApplyCaptivationActions(int startingVal)
    {
        int toreturn = startingVal;
        foreach (var @delegate in CaptivationActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public int ApplySleightOfHandActions(int startingVal)
    {
        int toreturn = startingVal;
        foreach (var @delegate in SleightOfHandActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public int ApplyPayoutActions(int startingVal)
    {
        int toreturn = startingVal;
        foreach (var @delegate in PayoutActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public int ApplyLiabilityActions(int startingVal)
    {
        int toreturn = startingVal;
        foreach (var @delegate in LiabilityActions.GetInvocationList())
        {
            var currAction = (Func<int, int>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public static ActionContainer operator +(ActionContainer a, ActionContainer b)
    {
        // Order doesn't matter for applying stats
        ActionContainer toreturn = new ActionContainer();
        List<ActionContainer> variables = new List<ActionContainer> { a, b };
        foreach(var actcon in variables)
        {
            foreach (var @delegate in actcon.CaptivationActions.GetInvocationList())
            {
                toreturn.CaptivationActions += (Func<int, int>)@delegate;
            }
            foreach (var @delegate in actcon.SleightOfHandActions.GetInvocationList())
            {
                toreturn.SleightOfHandActions += (Func<int, int>)@delegate;
            }
            foreach (var @delegate in actcon.PayoutActions.GetInvocationList())
            {
                toreturn.PayoutActions += (Func<int, int>)@delegate;
            }
            foreach (var @delegate in actcon.LiabilityActions.GetInvocationList())
            {
                toreturn.LiabilityActions += (Func<int, int>)@delegate;
            }
        }
        return toreturn;
    }
}
