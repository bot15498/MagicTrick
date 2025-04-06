using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
                scoreManager.sleightOfHand = Mathf.FloorToInt(scoreManager.sleightOfHand * MultiplyAmount);
                break;
            case Stats.Payout:
                scoreManager.additionalPayout = Mathf.FloorToInt(scoreManager.additionalPayout * MultiplyAmount);
                break;
            case Stats.Liability:
                scoreManager.liability = Mathf.FloorToInt(scoreManager.liability * MultiplyAmount);
                break;
        }
    }

    public override void PreviewAction(GameManager gameManager)
    {
        var scoreManager = gameManager.GetComponent<ScoreManager>();
        switch (StatToChange)
        {
            case Stats.Captivation:
                scoreManager.captivationToAdd = Mathf.FloorToInt(scoreManager.captivationToAdd * MultiplyAmount);
                break;
            case Stats.SleightOfHand:
                scoreManager.sleightOfHandToAdd = Mathf.FloorToInt(scoreManager.sleightOfHandToAdd * MultiplyAmount);
                break;
            case Stats.Payout:
                scoreManager.additionalPayoutToAdd = Mathf.FloorToInt(scoreManager.additionalPayoutToAdd * MultiplyAmount);
                break;
            case Stats.Liability:
                scoreManager.liabilityToAdd = Mathf.FloorToInt(scoreManager.liabilityToAdd * MultiplyAmount);
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
