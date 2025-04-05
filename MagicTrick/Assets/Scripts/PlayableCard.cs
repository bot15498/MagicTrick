using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum CardType
{ 
    Showmanship,
    Deception,
    Criminal,
    Cash
}

public enum Stats
{
    Captivation,
    SleightOfHand,
    Payout,
    Liability,
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class PlayableCard : ScriptableObject
{
    public string CardName;
    public string Description;
    public Sprite Image;
    public CardType Type;
    // The list of things this card will do (in order).
    public List<ActAction> Actions = new List<ActAction>();

    public void PlayCard()
    {
        // Note: these happen in the order they are added
        // Just make sure you put the multipliy actions last
        foreach (ActAction act in Actions)
        {
            //act.DoAction();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Add Increase Stat Action")]
    private void AddNewIncreaseStatAction()
    {
        IncreaseStatAction newaction = ScriptableObject.CreateInstance<IncreaseStatAction>();
        newaction.name = "Increase Stats";
        newaction.StatToChange = Stats.Captivation;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Add Multiply Stat Action")]
    private void AddNewMultiplyStatAction()
    {
        MultiplyStatAction newaction = ScriptableObject.CreateInstance<MultiplyStatAction>();
        newaction.name = "Multiply Stats";
        newaction.StatToChange = Stats.Captivation;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
#endif
}
