using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

    public override void PreviewAction(GameManager gameManager)
    {
        var scoreManager = gameManager.GetComponent<ScoreManager>();
        switch (StatToChange)
        {
            case Stats.Captivation:
                scoreManager.captivationToAdd += ChangeAmount;
                break;
            case Stats.SleightOfHand:
                scoreManager.sleightOfHandToAdd += ChangeAmount;
                break;
            case Stats.Payout:
                scoreManager.additionalPayoutToAdd += ChangeAmount;
                break;
            case Stats.Liability:
                scoreManager.liabilityToAdd += ChangeAmount;
                break;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Delete This")]
    private void DeleteThis()
    {
        Undo.DestroyObjectImmediate(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
