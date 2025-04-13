using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public long baseCaptivation = 0;
    public double baseSleightOfHand = 0;
    public long basePayout = 5;
    public long baseLiability = 0;
    public float basePayoutBonusFromScoreWeight = 0.5f;

    public long captivation = 0;
    public double sleightOfHand = 0;
    public long additionalPayout = 0;
    public long liability = 0;
    public float payoutBonusFromScoreWeight = 0.5f;

    public long previewCaptivation = 0;
    public double previewSleightOfHand = 0;
    public long previewAdditionalPayout = 0;
    public long previewLiability = 0;
    public float previewPayoutBonusFromScoreWeight = 0.5f;

    public long money = 0;

    public long Score
    {
        get
        {
            return (long)(captivation * (1 + sleightOfHand));
        }
    }

    public long PreviewScore
    {
        get
        {
            return (long)(previewCaptivation * (1 + previewSleightOfHand));
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void UpdateMoney(long requiredScore)
    {
        money += CalculatePayout(requiredScore) - liability;
    }

    public void ApplyToPreviewScore(ActionContainer container, GameManager gameManager)
    {
        // Also do the extra actions
        //container.ApplyNonStatActions(gameManager);

        previewCaptivation = container.ApplyCaptivationActions(captivation);
        previewSleightOfHand = Math.Max(container.ApplySleightOfHandActions(sleightOfHand), 0.0);
        previewAdditionalPayout = container.ApplyPayoutActions(additionalPayout);
        previewLiability = container.ApplyLiabilityActions(liability);

        //container.ApplyNonStatPostActions(gameManager);
    }

    public void ApplyToScore(ActionContainer container, GameManager gameManager)
    {
        // Also do the extra actions
        container.ApplyNonStatActions(gameManager);

        // Apply the contents of the container to the variables
        captivation = container.ApplyCaptivationActions(captivation);
        sleightOfHand = Math.Max(container.ApplySleightOfHandActions(sleightOfHand), 0.0);
        additionalPayout = container.ApplyPayoutActions(additionalPayout);
        liability = container.ApplyLiabilityActions(liability);

        container.ApplyNonStatPostActions(gameManager);
    }

    public long CalculatePayout(long requiredScore)
    {
        return additionalPayout + Mathf.FloorToInt((Score - requiredScore) / (requiredScore * payoutBonusFromScoreWeight));
    }

    public void ResetStats()
    {
        captivation = baseCaptivation;
        sleightOfHand = baseSleightOfHand;
        additionalPayout = basePayout;
        liability = baseLiability;
        payoutBonusFromScoreWeight = basePayoutBonusFromScoreWeight;
    }
}
