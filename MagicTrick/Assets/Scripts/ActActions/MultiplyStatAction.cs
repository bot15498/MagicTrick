using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyStatAction : ActAction
{
    public Stats StatToChange = Stats.Captivation;
    public float MultiplyAmount = 1f;

    public override void DoAction(GameManager gameManager)
    {
        var scoreManager = gameManager.GetComponent<ScoreManager>();
        switch (StatToChange)
        {
            case Stats.Captivation:
                scoreManager.captivation = Mathf.FloorToInt(scoreManager.captivation * MultiplyAmount);
                break;
            case Stats.SleightOfHand:
                scoreManager.sleightOfHand = Mathf.FloorToInt(scoreManager.captivation * MultiplyAmount);
                break;
            case Stats.Payout:
                scoreManager.additionalPayout = Mathf.FloorToInt(scoreManager.captivation * MultiplyAmount);
                break;
            case Stats.Liability:
                scoreManager.liability = Mathf.FloorToInt(scoreManager.captivation * MultiplyAmount);
                break;
        }
    }
}
