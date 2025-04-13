using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeceptionCheckSoH : ActAction
{
    public override ActionContainer AddAction(ActionContainer container, int slot, GameManager manager)
    {
        int numDeceptionCards = 0;
        foreach(var cardslot in manager.slots)
        {
            Card currcard = cardslot.GetComponentInChildren<Card>();
            if(currcard != null)
            {
                if(currcard.CardData.Type.Contains(CardType.Deception))
                {
                    numDeceptionCards++;
                }
            }
        }
        if(numDeceptionCards >= 2)
        {
            container.SleightOfHandActions += x => x + 4;
        }

        return container;
    }
}
