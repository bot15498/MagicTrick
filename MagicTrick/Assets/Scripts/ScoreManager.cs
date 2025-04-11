using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public long baseCaptivation = 0;
    public long baseSleightOfHand = 0;
    public long basePayout = 5;
    public long baseLiability = 0;
    public float basePayoutBonusFromScoreWeight = 0.5f;

    public long captivation = 0;
    public long sleightOfHand = 0;
    public long additionalPayout = 0;
    public long liability = 0;
    public float payoutBonusFromScoreWeight = 0.5f;

    public long previewCaptivation = 0;
    public long previewSleightOfHand = 0;
    public long previewAdditionalPayout = 0;
    public long previewLiability = 0;
    public float previewPayoutBonusFromScoreWeight = 0.5f;

    public long Score
    {
        get
        {
            return captivation * (1 + sleightOfHand);
        }
    }

    public long PreviewScore
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

    public void ApplyToScore(ActionContainer container, GameManager gameManager)
    {
        // Apply the contents of the container to the temp variables
        captivation = container.ApplyCaptivationActions(captivation);
        sleightOfHand = container.ApplySleightOfHandActions(sleightOfHand);
        additionalPayout = container.ApplyPayoutActions(additionalPayout);
        liability = container.ApplyLiabilityActions(liability);

        // Also do the extra actions
        container.ApplyNonStatActions(gameManager);
    }

    public long CalculatePayout(int requiredScore)
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
