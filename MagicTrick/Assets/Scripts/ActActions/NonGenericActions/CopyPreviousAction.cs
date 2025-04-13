using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPreviousAction : ActAction
{
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Go to the history, and pull out what ever is in -1 for this slot
        if(manager.historyManager.slotTimelines[slot].History.ContainsKey(-1))
        {
            container += manager.historyManager.slotTimelines[slot].History[-1];
        }
        return container;
    }
}
