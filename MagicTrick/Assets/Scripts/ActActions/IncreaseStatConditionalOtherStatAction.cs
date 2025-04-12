using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompareType
{
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    Equal
}

public class IncreaseStatConditionalOtherStatAction : ActAction
{
    public Stats StatToCheck = Stats.Captivation;
    public CompareType CompareType = CompareType.Equal;
    public double Threshold = 0; 
    public Stats StatToChange = Stats.Captivation;
    public double ChangeAmount;


    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Check to see if threshold is met.
        bool thresholdIsMet = true;
        if(StatToCheck == Stats.SleightOfHand)
        {
            // double math
            double valToCheck = manager.scoreManager.sleightOfHand;
            switch(CompareType)
            {
                case CompareType.Equal:
                    thresholdIsMet = valToCheck == Threshold;
                    break;
                case CompareType.LessThan:
                    thresholdIsMet = valToCheck < Threshold;
                    break;
                case CompareType.GreaterThan:
                    thresholdIsMet = valToCheck > Threshold;
                    break;
                case CompareType.LessThanOrEqual:
                    thresholdIsMet = valToCheck <= Threshold;
                    break;
                case CompareType.GreaterThanOrEqual:
                    thresholdIsMet = valToCheck >= Threshold;
                    break;
            }
        }
        else
        {
            // long math
            long valToCheck = 0;
            switch (StatToCheck)
            {
                case Stats.Captivation:
                    valToCheck = manager.scoreManager.captivation;
                    break;
                case Stats.Liability:
                    valToCheck = manager.scoreManager.liability;
                    break;
                case Stats.Payout:
                    valToCheck = manager.scoreManager.additionalPayout;
                    break;
            }
            switch (CompareType)
            {
                case CompareType.Equal:
                    thresholdIsMet = valToCheck == Threshold;
                    break;
                case CompareType.LessThan:
                    thresholdIsMet = valToCheck < Threshold;
                    break;
                case CompareType.GreaterThan:
                    thresholdIsMet = valToCheck > Threshold;
                    break;
                case CompareType.LessThanOrEqual:
                    thresholdIsMet = valToCheck <= Threshold;
                    break;
                case CompareType.GreaterThanOrEqual:
                    thresholdIsMet = valToCheck >= Threshold;
                    break;
            }
        }

        // Didn't match, so don't add anything.
        if(!thresholdIsMet)
        {
            return container;
        }

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
