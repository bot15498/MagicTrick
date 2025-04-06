using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prop", menuName = "Prop")]
public class PlayableProp : ScriptableObject
{
    public string PropName;
    public string Description;
    public Sprite Image;

    public List<SlotAction> SlotActions = new List<SlotAction>();

    public void ApplyProp(GameManager gameManager, int slot)
    {
        foreach (ActAction act in SlotActions[slot].Actions)
        {
            act.DoAction(gameManager);
        }
    }

    public void PreviewProp(GameManager gameManager, int slot)
    {
        foreach (ActAction act in SlotActions[slot].Actions)
        {
            act.PreviewAction(gameManager);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Add Slot Action")]
    private void AddSlotAction()
    {
        SlotAction newslotAction = CreateInstance<SlotAction>();
        newslotAction.name = "Slot Action";

        SlotActions.Add(newslotAction);

        AssetDatabase.AddObjectToAsset(newslotAction, this);
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newslotAction);
    }
#endif
}
