using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class UniformRandomChanceAction : ActAction
{
    public List<ActAction> UniformActionChance = new List<ActAction>();

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        var rand = new System.Random();
        var roll = rand.Next(0, UniformActionChance.Count);
        return UniformActionChance[roll].AddAction(container, slot, manager);
    }

#if UNITY_EDITOR
    [ContextMenu("Add Increase Stat Action")]
    private void AddNewIncreaseStatAction()
    {
        IncreaseStatAction newaction = CreateInstance<IncreaseStatAction>();
        newaction.name = "Increase Stats";
        newaction.StatToChange = Stats.Captivation;

        UniformActionChance.Add(newaction);

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

        UniformActionChance.Add(newaction);

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

        UniformActionChance.Add(newaction);

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

        UniformActionChance.Add(newaction);

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

        UniformActionChance.Add(newaction);

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

        UniformActionChance.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
#endif
}
