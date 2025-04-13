using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapToLiabilityAction : ActAction
{

    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        // Check if only prop by seeing if number of props == 1
        int propcount = 0;
        foreach(var propslot in manager.propSlots)
        {
            Prop currPropObj = propslot.GetComponentInChildren<Prop>();
            if(currPropObj != null)
            {
                propcount++;
            }
        }
        if(propcount <= 1)
        {
            long toadd = manager.scoreManager.liability / 2;
            container.CaptivationActions += x => x + toadd;
            container.LiabilityActions += x => x - toadd;
        }
        return container;
    }
}
