using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AddToFutureActAction : ActAction
{
    // 0 is current step, 1 is one act in the future. 
    public int RelativeFutureStep = 1;
    public List<ActAction> FutureActions = new List<ActAction>();
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Add to post actions to add a +2 in the future
        container.NonStatPostActions += x =>
        {
            ActionContainer cont;
            if (manager.historyManager.slotTimelines[slot].History.ContainsKey(RelativeFutureStep))
            {
                cont = manager.historyManager.slotTimelines[slot].History[RelativeFutureStep];
            }
            else
            {
                cont = new ActionContainer();
            }
            foreach(var act in FutureActions)
            {
                cont = act.AddAction(cont, slot, manager);
            }
            manager.historyManager.slotTimelines[slot].AddToTimeline(cont, RelativeFutureStep);
            return 0;
        };

        // Don't add anything to preview actions, not needed.

        return container;
    }
#if UNITY_EDITOR
    [ContextMenu("Add Increase Stat Action")]
    private void AddNewIncreaseStatAction()
    {
        IncreaseStatAction newaction = CreateInstance<IncreaseStatAction>();
        newaction.name = "Increase Stats";
        newaction.StatToChange = Stats.Captivation;

        FutureActions.Add(newaction);

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

        FutureActions.Add(newaction);

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

        FutureActions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
#endif
}
