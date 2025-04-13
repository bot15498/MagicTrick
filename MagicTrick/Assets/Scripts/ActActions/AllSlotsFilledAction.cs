using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AllSlotsFilledAction : ActAction
{
    public List<ActAction> AdditionalActions = new List<ActAction>();
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // check if all actions are filled
        int numCards = 0;
        int numProps = 0;
        foreach(var cardSlot in manager.slots)
        {
            Card cardobj = cardSlot.GetComponentInChildren<Card>();
            if (cardobj != null)
            {
                numCards++;
            }
        }
        foreach (var propslot in manager.propSlots)
        {
            Prop propobj = propslot.GetComponentInChildren<Prop>();
            if (propobj != null)
            {
                numProps++;
            }
        }

        // Check if all are filled.
        if (numCards == manager.slots.Count && numProps == manager.propSlots.Count)
        {
            foreach(var act in AdditionalActions)
            {
                container = act.AddAction(container, slot, manager);
            }
        }

        return container;
    }

#if UNITY_EDITOR
    [ContextMenu("Add Increase Stat Action")]
    private void AddNewIncreaseStatAction()
    {
        IncreaseStatAction newaction = CreateInstance<IncreaseStatAction>();
        newaction.name = "Increase Stats";
        newaction.StatToChange = Stats.Captivation;

        AdditionalActions.Add(newaction);

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

        AdditionalActions.Add(newaction);

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

        AdditionalActions.Add(newaction);

        AssetDatabase.AddObjectToAsset(newaction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newaction);
    }
#endif
}
