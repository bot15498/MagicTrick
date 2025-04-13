using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

    public ActionContainer ApplyProp(ActionContainer container, GameManager gameManager, int currCardSlot, int currPropSlot)
    {
        // Apply the actions for this current prop
        if (currCardSlot == currPropSlot)
        {
            foreach (ActAction act in Actions)
            {
                container = act.AddAction(container, currCardSlot, gameManager);
            }
        }
        // Apply actions to the previous slot's trick if it exists
        if (currCardSlot == currPropSlot - 1)
        {
            foreach (ActAction act in PreviousSlotActions)
            {
                container = act.AddAction(container, currCardSlot, gameManager);
            }
        }
        // Apply actions to the next slot's trick if it exists
        if (currCardSlot == currPropSlot + 1)
        {
            foreach (ActAction act in NextSlotActions)
            {
                container = act.AddAction(container, currCardSlot, gameManager);
            }
        }
        // Apply actions fixed slots
        for (int i = 0; i < FixedSlotActions.Count; i++)
        {
            if(currCardSlot == i)
            {
                foreach(ActAction act in FixedSlotActions[i].Actions)
                {
                    container = act.AddAction(container, currCardSlot, gameManager);
                }
            }
        }

        return container;
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
