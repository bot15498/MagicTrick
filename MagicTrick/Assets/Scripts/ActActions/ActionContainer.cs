using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContainer
{
    // This is a holder for all the actions you are going to do to the stats this timestep
    public Func<long, long> CaptivationActions { get; set; } = x => x;
    public Func<long, long> SleightOfHandActions { get; set; } = x => x;
    public Func<long, long> PayoutActions { get; set; } = x => x;
    public Func<long, long> LiabilityActions { get; set; } = x => x;

    public long ApplyCaptivationActions(long startingVal)
    {
        long toreturn = startingVal;
        foreach (var @delegate in CaptivationActions.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public long ApplySleightOfHandActions(long startingVal)
    {
        long toreturn = startingVal;
        foreach (var @delegate in SleightOfHandActions.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public long ApplyPayoutActions(long startingVal)
    {
        long toreturn = startingVal;
        foreach (var @delegate in PayoutActions.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
            toreturn = currAction(toreturn);
        }
        return toreturn;
    }

    public long ApplyLiabilityActions(long startingVal)
    {
        long toreturn = startingVal;
        foreach (var @delegate in LiabilityActions.GetInvocationList())
        {
            var currAction = (Func<long, long>)@delegate;
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
                toreturn.CaptivationActions += (Func<long, long>)@delegate;
            }
            foreach (var @delegate in actcon.SleightOfHandActions.GetInvocationList())
            {
                toreturn.SleightOfHandActions += (Func<long, long>)@delegate;
            }
            foreach (var @delegate in actcon.PayoutActions.GetInvocationList())
            {
                toreturn.PayoutActions += (Func<long, long>)@delegate;
            }
            foreach (var @delegate in actcon.LiabilityActions.GetInvocationList())
            {
                toreturn.LiabilityActions += (Func<long, long>)@delegate;
            }
        }
        return toreturn;
    }
}
