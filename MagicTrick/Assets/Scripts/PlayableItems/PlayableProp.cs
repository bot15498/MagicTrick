using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prop", menuName = "Prop")]
public class PlayableProp : PlayableItem
{
    public string PropName;
    public string Description;
    public Sprite Image;

    [Header("Actions Applied to Previous and Next Slots")]
    public List<ActAction> PreviousSlotActions = new List<ActAction>();
    public List<ActAction> NextSlotActions = new List<ActAction>();

    [Header("Actions applied to Fixed slots")]
    public List<SlotAction> FixedSlotActions = new List<SlotAction>();

    public void ApplyProp(GameManager gameManager, int slot)
    {
        // Apply the current slot actions
        foreach (ActAction act in Actions)
        {
            act.DoAction(gameManager);
        }
        // Apply the fixed slot actions
        foreach (ActAction act in FixedSlotActions[slot].Actions)
        {
            act.DoAction(gameManager);
        }
    }

    public void PreviewProp(GameManager gameManager, int slot)
    {
        // Apply the current slot actions
        if (Actions != null)
        {
            foreach (ActAction act in Actions)
            {
                act.PreviewAction(gameManager);
            }
        }
        // Apply the fixed slot actions
        if (FixedSlotActions != null && slot < FixedSlotActions.Count)
        {
            foreach (ActAction act in FixedSlotActions[slot].Actions)
            {
                act.PreviewAction(gameManager);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Add Slot Actions")]
    private void AddSlotAction()
    {
        // Adds three lists for the three slots
        for (int i = 0; i < 3; i++)
        {
            SlotAction newslotAction = CreateInstance<SlotAction>();
            newslotAction.name = "Slot Action";
            newslotAction.SlotNumber = i;

            FixedSlotActions.Add(newslotAction);

            AssetDatabase.AddObjectToAsset(newslotAction, this);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(newslotAction);
        }
    }
#endif
}
