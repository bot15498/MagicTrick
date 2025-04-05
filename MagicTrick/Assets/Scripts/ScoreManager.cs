using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int baseCaptivation = 0;
    public int baseSleightOfHand = 0;
    public int basePayout = 5;
    public int baseLiability = 0;
    public int captivation = 0;
    public int sleightOfHand = 0;
    public int additionalPayout = 0;
    public int liability = 0;
    public float payoutBonusFromScoreWeight = 0.5f;
    public int Score
    {
        get
        {
            return captivation * sleightOfHand;
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
        return basePayout + additionalPayout + Mathf.FloorToInt( (Score - requiredScore) / (requiredScore * payoutBonusFromScoreWeight) );
    }
}
