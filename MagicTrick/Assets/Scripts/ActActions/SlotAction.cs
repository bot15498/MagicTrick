using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SlotAction : ScriptableObject
{
    public int SlotNumber;
    public List<ActAction> Actions = new List<ActAction>();

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
#endif
}
