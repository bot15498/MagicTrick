using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public abstract class PlayableItem : ScriptableObject
{
    // The list of things this card will do (in order).
    [Header("Actions Applied to Current Slot")]
    public List<ActAction> Actions = new List<ActAction>();
    public bool DoesRandomAction = false;

#if UNITY_EDITOR
    [ContextMenu("Add Increase Stat Action")]
    private void AddNewIncreaseStatAction()
    {
        IncreaseStatAction newaction = CreateInstance<IncreaseStatAction>();
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
        MultiplyStatAction newaction = CreateInstance<MultiplyStatAction>();
        newaction.name = "Multiply Stats";
        newaction.StatToChange = Stats.Captivation;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Add Multiply Current Effect Action")]
    private void AddNewMultiplyCurrentEffect()
    {
        MultiplyCurrentEffectAction newaction = CreateInstance<MultiplyCurrentEffectAction>();
        newaction.name = "Multiply Current Effect";
        newaction.MultiplyAmount = 1;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Add Disable Future Slot")]
    private void AddDisableFutureSlotAction()
    {
        DisableFutureSlotAction newaction = CreateInstance<DisableFutureSlotAction>();
        newaction.name = "Disable Future Slot";

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Multiply Next Trick")]
    private void AddMultiplyNextTrick()
    {
        MultiplyOtherAction newaction = CreateInstance<MultiplyOtherAction>();
        newaction.name = "Multiply Next Trick";
        newaction.MultiplyAmount = 1;
        newaction.RelativeTimestep = 1;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Increase Stat On Turn")]
    private void AddIncreaseStatOnTurnAction()
    {
        IncreaseStatOnTurnAction newaction = CreateInstance<IncreaseStatOnTurnAction>();
        newaction.name = "Increase Stat On Specific Turn";
        newaction.StatToChange = Stats.Captivation;
        newaction.ChangeAmount = 0;
        newaction.TargetAct = 1;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Uniform Random Action Choice")]
    private void AddUniformRandomActionChoice()
    {
        UniformRandomChanceAction newaction = CreateInstance<UniformRandomChanceAction>();
        newaction.name = "Choose Uniform Random Action";

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Increase Stat If Card Matches Type")]
    private void AddIncreaseStatIfCardMatchesType()
    {
        IncreaseStatConditionalCardAction newaction = CreateInstance<IncreaseStatConditionalCardAction>();
        newaction.name = "Increase Stat If CardType Matches";
        newaction.CardTypeToLookFor = CardType.Showmanship;
        newaction.StatToChange = Stats.Captivation;
        newaction.ChangeAmount = 0;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
    [ContextMenu("Increase Stat If Stat Matches Threshold")]
    private void AddIncreaseStatIfThreshold()
    {
        IncreaseStatConditionalOtherStatAction newaction = CreateInstance<IncreaseStatConditionalOtherStatAction>();
        newaction.name = "Increase Stat If Stat Threshold is met";
        newaction.StatToCheck = Stats.Captivation;
        newaction.CompareType = CompareType.Equal;
        newaction.StatToChange = Stats.Captivation;
        newaction.ChangeAmount = 0;

        Actions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
#endif
}
