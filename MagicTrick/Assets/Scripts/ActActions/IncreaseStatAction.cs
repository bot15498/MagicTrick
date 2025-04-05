using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStatAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public int ChangeAmount = 0;

    public override void DoAction(GameManager gameManager)
    {
        var scoreManager = gameManager.GetComponent<ScoreManager>();
        switch(StatToChange)
        {
            case Stats.Captivation:
                scoreManager.captivation += ChangeAmount;
                break;
            case Stats.SleightOfHand:
                scoreManager.sleightOfHand += ChangeAmount;
                break;
            case Stats.Payout:
                scoreManager.additionalPayout += ChangeAmount;
                break;
            case Stats.Liability:
                scoreManager.liability += ChangeAmount;
                break;
        }
    }
}
