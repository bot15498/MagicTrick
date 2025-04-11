using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFutureSlotAction : ActAction
{
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Add to the extra actions for this turn to disable the slot
        container.NonStatActions += (GameManager x) =>
        {
            x.SetSlotEnable(slot, false);

            // at the same time, add a action in the future to reenable the slot
            ActionContainer future = new ActionContainer();
            future.NonStatActions += x =>
            {
                x.SetSlotEnable(slot, true);
                return 0;
            };
            x.historyManager.slotTimelines[slot].AddToTimeline(future, slot);
            return 0;
        };
        return container;
    }
}
