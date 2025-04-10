using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int baseCaptivation = 0;
    public int baseSleightOfHand = 0;
    public int basePayout = 5;
    public int baseLiability = 0;
    public float basePayoutBonusFromScoreWeight = 0.5f;

    public int captivation = 0;
    public int sleightOfHand = 0;
    public int additionalPayout = 0;
    public int liability = 0;
    public float payoutBonusFromScoreWeight = 0.5f;

    public int previewCaptivation = 0;
    public int previewSleightOfHand = 0;
    public int previewAdditionalPayout = 0;
    public int previewLiability = 0;
    public float previewPayoutBonusFromScoreWeight = 0.5f;

    public int Score
    {
        get
        {
            return captivation * (1 + sleightOfHand);
        }
    }

    public int PreviewScore
    {
        get
        {
            return previewCaptivation * (1 + previewSleightOfHand);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void ApplyToPreviewScore(ActionContainer container)
    {
        previewCaptivation = container.ApplyCaptivationActions(captivation);
        previewSleightOfHand = container.ApplySleightOfHandActions(sleightOfHand);
        previewAdditionalPayout = container.ApplyPayoutActions(additionalPayout);
        previewLiability = container.ApplyLiabilityActions(liability);
    }

    public void ApplyToScore(ActionContainer container)
    {
        // Apply the contents of the container to the temp variables
        captivation = container.ApplyCaptivationActions(captivation);
        sleightOfHand = container.ApplySleightOfHandActions(sleightOfHand);
        additionalPayout = container.ApplyPayoutActions(additionalPayout);
        liability = container.ApplyLiabilityActions(liability);
    }

    public int CalculatePayout(int requiredScore)
    {
        return basePayout + additionalPayout + Mathf.FloorToInt((Score - requiredScore) / (requiredScore * payoutBonusFromScoreWeight));
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
