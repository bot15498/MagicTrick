using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncrementDoubleText : MonoBehaviour
{
    public bool isUpdating = false;
    public double CurrentValue = 0;
    public double TargetValue = 0;
    public double stepVal = 3;
    public string prefix;
    private TMP_Text targetText;

    // Start is called before the first frame update
    void Start()
    {
        targetText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
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
