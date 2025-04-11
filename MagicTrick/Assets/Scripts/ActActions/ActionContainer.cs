using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContainer
{
    // This is a holder for all the actions you are going to do to the stats this timestep
    public Func<long, long> CaptivationActions { get; set; } = x => x;
    public Func<double, double> SleightOfHandActions { get; set; } = x => x;
    public Func<long, long> PayoutActions { get; set; } = x => x;
    public Func<long, long> LiabilityActions { get; set; } = x => x;
    // Non stat actions only get run when playing a card
    public Func<GameManager, int> NonStatActions { get; set; } = x => 0;
    // Non stat actions only get run when playing a card, but happen at the end
    public Func<GameManager, int> NonStatPostActions { get; set; } = x => 0;

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

    public double ApplySleightOfHandActions(double startingVal)
    {
        double toreturn = startingVal;
        foreach (var @delegate in SleightOfHandActions.GetInvocationList())
        {
            var currAction = (Func<double, double>)@delegate;
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

    public int ApplyNonStatActions(GameManager gameManager)
    {
        return NonStatActions(gameManager);
    }

    public int ApplyNonStatPostActions(GameManager gameManager)
    {
        return NonStatPostActions(gameManager);
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
                toreturn.SleightOfHandActions += (Func<double, double>)@delegate;
            }
            foreach (var @delegate in actcon.PayoutActions.GetInvocationList())
            {
                toreturn.PayoutActions += (Func<long, long>)@delegate;
            }
            foreach (var @delegate in actcon.LiabilityActions.GetInvocationList())
            {
                toreturn.LiabilityActions += (Func<long, long>)@delegate;
            }
            foreach(var @delegate in actcon.NonStatActions.GetInvocationList())
            {
                toreturn.NonStatActions += (Func<GameManager, int>)@delegate;
            }
        }
        return toreturn;
    }
}
