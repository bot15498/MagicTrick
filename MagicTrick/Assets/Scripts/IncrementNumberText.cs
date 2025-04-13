using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncrementNumberText : MonoBehaviour
{
    public bool isUpdating = false;
    public long CurrentValue = 0;
    public long TargetValue = 0;
    private long stepVal = 1;
    public string prefix;
    private TMP_Text targetText;
    void Start()
    {
        targetText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        stepVal = (long)(Mathf.Sqrt(TargetValue - CurrentValue) /10);
        stepVal = stepVal < 1 ? 1 : stepVal;
        if (CurrentValue != TargetValue)
        {
            isUpdating = true;
            // Updates every frame
            if (CurrentValue < TargetValue)
            {
                CurrentValue += stepVal;
                CurrentValue = CurrentValue > TargetValue ? TargetValue : CurrentValue; 
            }
            else
            {
                CurrentValue -= stepVal;
                CurrentValue = CurrentValue < TargetValue ? TargetValue : CurrentValue;
            }
            targetText.text = $"{prefix}{CurrentValue:n0}";
        }
        else
        {
            isUpdating = false;
            targetText.text = $"{prefix}{TargetValue:n0}";
        }
    }
}
