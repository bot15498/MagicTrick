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

    public int captivationToAdd = 0;
    public int sleightOfHandToAdd = 0;
    public int additionalPayoutToAdd = 0;
    public int liabilityToAdd = 0;
    public int Score
    {
        get
        {
            return captivation * (1 + sleightOfHand);
        }
    }
    public int TemporaryScore
    {
        get
        {
            return (captivation + captivationToAdd) * (1 + sleightOfHand + sleightOfHandToAdd);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public int CalculatePayout(int requiredScore)
    {
        return basePayout + additionalPayout + Mathf.FloorToInt((Score - requiredScore) / (requiredScore * payoutBonusFromScoreWeight));
    }

    public void ClearToAddVariables()
    {
        captivationToAdd = 0;
        sleightOfHandToAdd = 0;
        additionalPayoutToAdd = 0;
        liabilityToAdd = 0;
    }
}
