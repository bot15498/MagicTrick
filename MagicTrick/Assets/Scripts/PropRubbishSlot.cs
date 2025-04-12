using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRubbishSlot : PropZone
{
    public override void AssignProp(Prop newProp)
    {
        // Delete the prop
        Destroy(newProp.transform.parent.gameObject);
    }
}
