using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertStatToStatAction : ActAction
{
    public Stats StatToConvertFrom = Stats.Captivation;
    public Stats StatToConvertTo = Stats.Captivation;

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Put in other actions
        container.NonStatPostActions += x =>
        {
            long changeVal = 0;
            double changeVal2 = 0;
            switch (StatToConvertFrom)
            {
                case Stats.Captivation:
                    changeVal = manager.scoreManager.captivation;
                    break;
                case Stats.SleightOfHand:
                    changeVal2 = manager.scoreManager.sleightOfHand;
                    break;
                case Stats.Payout:
                    changeVal = manager.scoreManager.additionalPayout;
                    break;
                case Stats.Liability:
                    changeVal = manager.scoreManager.liability;
                    break;
            }
            switch (StatToConvertTo)
            {
                case Stats.Captivation:
                    manager.scoreManager.captivation += changeVal != 0 ? changeVal : (long)changeVal2;
                    break;
                case Stats.SleightOfHand:
                    manager.scoreManager.sleightOfHand += changeVal != 0 ? changeVal : (long)changeVal2; ;
                    break;
                case Stats.Payout:
                    manager.scoreManager.additionalPayout += changeVal != 0 ? changeVal : (long)changeVal2; ;
                    break;
                case Stats.Liability:
                    manager.scoreManager.liability += changeVal != 0 ? changeVal : (long)changeVal2; ;
                    break;
            }
            return 0;
        };

        return container;
    }
}
